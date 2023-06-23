﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Visitor;

namespace MoBi.Presentation.Tasks
{
   public interface ICheckNameVisitor
   {
      /// <summary>
      ///    Returns the changes required to be performed in the <paramref name="renamingContext" /> when renaming the
      ///    <paramref name="objectToRename" />
      /// </summary>
      /// <param name="objectToRename">Object to rename</param>
      /// <param name="newName">New name for the object</param>
      /// <param name="renamingContext">Context where the renaming applies (e.g. project but could also be a simple container)</param>
      /// <param name="oldName"></param>
      IReadOnlyList<IStringChange> GetPossibleChangesFrom(IObjectBase objectToRename, string newName, IObjectBase renamingContext, string oldName);
   }

   public class CheckNameVisitor : ICheckNameVisitor,
      IVisitor<IObjectBase>,
      IVisitor<IFormula>,
      IVisitor<ParameterValue>,
      IVisitor<InitialCondition>,
      IVisitor<ObserverBuilder>,
      IVisitor<ReactionBuilder>,
      IVisitor<ApplicationBuilder>,
      IVisitor<MoleculeBuilder>,
      IVisitor<EventAssignmentBuilder>,
      IVisitor<IMoBiSimulation>,
      IVisitor<IBuildingBlock>,
      IVisitor<TransporterMoleculeContainer>,
      IVisitor<TransportBuilder>,
      IVisitor<ApplicationMoleculeBuilder>,
      IVisitor<SimulationSettings>,
      IVisitor<EventGroupBuilder>
   {
      private readonly StringChanges _changes = new StringChanges();
      private string _newName;
      private IObjectBase _objectToRename;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IAliasCreator _aliasCreator;
      private readonly string _namePropertyName;
      private readonly string _eventObjectPathPropertyName;
      private readonly string _appBuilderMoleculeNamePropertyName;
      private readonly string _reactionPartnerMoleculeNamePropertyName;
      private IBuildingBlock _buildingBlock;
      private readonly string _transportNamePropertyName;
      private string _oldName;
      private readonly IParameterValuePathTask _parameterValuePathTask;
      private readonly IInitialConditionPathTask _msvPathTask;
      private readonly ICloneManager _cloneManager;

      public CheckNameVisitor(IObjectTypeResolver objectTypeResolver, IAliasCreator aliasCreator, IParameterValuePathTask parameterValuePathTask, IInitialConditionPathTask msvPathTask, ICloneManager cloneManager)
      {
         _objectTypeResolver = objectTypeResolver;
         _aliasCreator = aliasCreator;
         _parameterValuePathTask = parameterValuePathTask;
         _msvPathTask = msvPathTask;
         _cloneManager = cloneManager;

         Expression<Func<IObjectBase, string>> nameString = x => x.Name;
         _namePropertyName = nameString.Name();

         Expression<Func<ApplicationBuilder, string>> appBuilderMoleculeName = x => x.MoleculeName;
         _appBuilderMoleculeNamePropertyName = appBuilderMoleculeName.Name();

         Expression<Func<ReactionPartnerBuilder, string>> reactionPartnerMoleculeName = x => x.MoleculeName;
         _reactionPartnerMoleculeNamePropertyName = reactionPartnerMoleculeName.Name();

         Expression<Func<EventAssignmentBuilder, ObjectPath>> eventObjectPath = x => x.ObjectPath;
         _eventObjectPathPropertyName = eventObjectPath.Name();

         Expression<Func<TransporterMoleculeContainer, string>> transportName = x => x.TransportName;
         _transportNamePropertyName = transportName.Name();
      }

      public IReadOnlyList<IStringChange> GetPossibleChangesFrom(IObjectBase objectToRename, string newName, IObjectBase renamingContext, string oldName)
      {
         _changes.Clear();
         _objectToRename = objectToRename;
         _newName = newName;
         _oldName = oldName;

         try
         {
            renamingContext.AcceptVisitor(this);
            return _changes.ToList();
         }
         finally
         {
            _buildingBlock = null;
            _objectToRename = null;
            _changes.Clear();
         }
      }

      public void Visit(IFormula formula)
      {
         checkObjectBase(formula);
         // create aliases from name to change them accordingly
         var oldAlias = _aliasCreator.CreateAliasFrom(_oldName);
         var newAlias = _aliasCreator.CreateAliasFrom(_newName);
         foreach (var path in formula.ObjectPaths)
         {
            // possible path adjustments
            if (path.Contains(_oldName))
            {
               //var newPath = generateNewPath(path);
               _changes.Add(formula, _buildingBlock, new ChangePathElementAtFormulaUseablePathCommand(_newName, formula, _oldName, path, _buildingBlock));
            }

            // possible alias Adjustments
            if (path.Alias.Equals(oldAlias))
            {
               var i = 0;
               // change new alias and remember this for this formula to change formula strign right if nessesary
               while (formula.ObjectPaths.Select(x => x.Alias).Contains(newAlias))
               {
                  newAlias = $"{newAlias}{i}";
                  i++;
               }

               _changes.Add(formula, _buildingBlock, new EditFormulaAliasCommand(formula, newAlias, oldAlias, _buildingBlock));
            }
         }

         var explicitFormula = formula as ExplicitFormula;
         if (explicitFormula == null) return;

         // possible Formula string adjustments
         if (!containsWord(explicitFormula.FormulaString, oldAlias))
            return;

         var newFormulaString = wordReplace(explicitFormula.FormulaString, oldAlias, newAlias);
         var editFormulaStringCommand = new EditFormulaStringCommand(newFormulaString, explicitFormula, _buildingBlock);
         _changes.Add(formula, _buildingBlock,
            editFormulaStringCommand,
            editFormulaStringCommand.Description);
      }

      private string stringReplace(string replaceIn)
      {
         return replaceIn.Replace(_oldName, _newName);
      }

      private bool containsOldName(string stringToCheck)
      {
         return stringToCheck.Contains(_oldName);
      }

      private string wordReplace(string replaceIn, string oldValue, string newValue)
      {
         var pattern = $"\\b{oldValue}\\b";
         return Regex.Replace(replaceIn, pattern, newValue);
      }

      private bool containsWord(string stringToCheck, string value)
      {
         var pattern = $"\\b{value}\\b";
         return Regex.IsMatch(stringToCheck, pattern);
      }

      public void Visit(IObjectBase objectBase)
      {
         checkObjectBase(objectBase);
      }

      private void checkObjectBase<T>(T objectBase) where T : IObjectBase
      {
         if (objectBase.IsAnImplementationOf<IContainer>())
            checkTagsInContainer((IContainer)objectBase);

         if (_objectToRename.Equals(objectBase))
            return;

         if (string.Equals(_oldName, objectBase.Name))
         {
            _changes.Add(objectBase, _buildingBlock, new EditObjectBasePropertyInBuildingBlockCommand(_namePropertyName, _newName, _oldName, objectBase, _buildingBlock),
               AppConstants.Commands.EditDescription(_objectTypeResolver.TypeFor<T>(), _namePropertyName, _oldName, _newName, objectBase.Name));
         }
      }

      private void checkTagsInContainer(IContainer container)
      {
         //Check Name Tags!
         foreach (var tag in container.Tags)
         {
            if (tag.Value.Equals(_oldName))
            {
               _changes.Add(container, _buildingBlock, new ChangeContainerTagCommand(_newName, _oldName, container, _buildingBlock),
                  AppConstants.Commands.EditDescription(ObjectTypes.Container, "Tag", _oldName, _newName, container.Name));
            }
         }
      }

      private ObjectPath generateNewPath(ObjectPath containerPath)
      {
         var newPath = new ObjectPath();
         foreach (var element in containerPath)
         {
            newPath.Add(element.Equals(_oldName) ? _newName : element);
         }

         return newPath;
      }

      public void Visit(IUsingFormula usingFormula)
      {
         checkObjectBase(usingFormula);
         // Formula is not checked here, only checked in Chache cause double will cause errors in undo
      }

      public void Visit(TransportBuilder transportBuilder)
      {
         checkMoleculeDependentBuilder(transportBuilder, _objectTypeResolver.TypeFor(transportBuilder));
         checkDescriptorCriteria(transportBuilder, x => x.SourceCriteria);
         checkDescriptorCriteria(transportBuilder, x => x.TargetCriteria);
      }

      private void checkMoleculeDependentBuilder(IMoleculeDependentBuilder transportBuilder, string objectType)
      {
         checkObjectBase(transportBuilder);
         if (transportBuilder.MoleculeNames().Contains(_oldName))
         {
            _changes.Add(transportBuilder, _buildingBlock,
               new ChangeMoleculeNameAtMoleculeDependentBuilderCommand(_newName, _oldName, transportBuilder, _buildingBlock) { ObjectType = objectType });
         }

         if (transportBuilder.MoleculeNamesToExclude().Contains(_oldName))
         {
            _changes.Add(transportBuilder, _buildingBlock,
               new ChangeExcludeMoleculeNameAtMoleculeDependentBuilderCommand(_newName, _oldName, transportBuilder, _buildingBlock) { ObjectType = objectType });
         }
      }

      public void Visit(ObserverBuilder observerBuilder)
      {
         checkMoleculeDependentBuilder(observerBuilder, ObjectTypes.ObserverBuilder);
         checkDescriptorCriteria(observerBuilder, x => x.ContainerCriteria);
      }

      private void checkDescriptorCriteria<T>(T taggedObject, Func<T, DescriptorCriteria> descriptorCriteriaRetriever) where T : class, IObjectBase
      {
         foreach (var tagCondition in descriptorCriteriaRetriever(taggedObject).OfType<ITagCondition>())
         {
            if (!string.Equals(tagCondition.Tag, _oldName))
               continue;

            var commandParameters = new TagConditionCommandParameters<T> { TaggedObject = taggedObject, BuildingBlock = _buildingBlock, DescriptorCriteriaRetriever = descriptorCriteriaRetriever };
            _changes.Add(taggedObject, _buildingBlock, new EditTagCommand<T>(_newName, _oldName, commandParameters));
         }
      }

      public void Visit(EventGroupBuilder eventGroupBuilder)
      {
         checkObjectBase(eventGroupBuilder);
         checkDescriptorCriteria(eventGroupBuilder, x => x.SourceCriteria);
      }

      public void Visit(ReactionBuilder reaction)
      {
         checkObjectBase(reaction);
         checkReactionPartnerIn(reaction.Educts, reaction, educt: true);
         checkReactionPartnerIn(reaction.Products, reaction, educt: false);
         checkModifier(reaction);
         checkDescriptorCriteria(reaction, x => x.ContainerCriteria);
      }

      private void checkModifier(ReactionBuilder reaction)
      {
         if (!reaction.ModifierNames.Contains(_oldName)) return;

         var reactionBuildingBlock = getReactionBuildingBlockContaining(reaction);
         _changes.Add(reaction, reactionBuildingBlock, new ChangeModifierCommand(_newName, _oldName, reaction, reactionBuildingBlock));
      }

      private MoBiReactionBuildingBlock getReactionBuildingBlockContaining(ReactionBuilder reaction)
      {
         return _buildingBlock as MoBiReactionBuildingBlock;
      }

      private void checkReactionPartnerIn(IEnumerable<ReactionPartnerBuilder> reactionPartners, ReactionBuilder reaction, bool educt)
      {
         foreach (var reactionPartner in reactionPartners.Where(reactionPartner => reactionPartner.MoleculeName.Equals(_oldName)))
         {
            var command = new EditReactionPartnerMoleculeNameCommand(_newName, reaction, reactionPartner, getReactionBuildingBlockContaining(reaction));
            _changes.Add(reactionPartner, _buildingBlock, command);
         }
      }

      public void Visit(ApplicationBuilder applicationBuilder)
      {
         Visit(applicationBuilder.DowncastTo<EventGroupBuilder>());

         if (!string.Equals(applicationBuilder.MoleculeName, _oldName))
            return;

         _changes.Add(applicationBuilder, _buildingBlock,
            new EditObjectBasePropertyInBuildingBlockCommand(_appBuilderMoleculeNamePropertyName, _newName, _oldName, applicationBuilder, _buildingBlock),
            AppConstants.Commands.EditDescription(ObjectTypes.Application, _appBuilderMoleculeNamePropertyName, _oldName, _newName, applicationBuilder.Name));
      }

      public void Visit(MoleculeBuilder moleculeBuilder)
      {
         checkObjectBase(moleculeBuilder);
         // Formula is not checked here, only checked in cache cause double will cause errors in undo
      }

      public void Visit(EventAssignmentBuilder eventAssignmentBuilder)
      {
         Visit(eventAssignmentBuilder as IUsingFormula);
         if (!eventAssignmentBuilder.ObjectPath.Contains(_oldName)) return;

         var oldPath = eventAssignmentBuilder.ObjectPath;
         var newPath = generateNewPath(eventAssignmentBuilder.ObjectPath);
         _changes.Add(eventAssignmentBuilder, _buildingBlock, new EditObjectBasePropertyInBuildingBlockCommand(_eventObjectPathPropertyName, newPath, oldPath, eventAssignmentBuilder, _buildingBlock));
      }

      public void Visit(IMoBiSimulation simulation)
      {
         //Visit the specified Simulation. Does Nothing here because a simulation should not be renamed
      }

      public void Visit(IBuildingBlock buildingBlock)
      {
         checkBuildingBlock(buildingBlock);
      }

      public void Visit(SimulationSettings simulationSettings)
      {
         checkBuildingBlock(simulationSettings);
         checkOutputSelectionIn(simulationSettings);
         checkChartTemplatesIn(simulationSettings);
      }

      private void checkChartTemplatesIn(SimulationSettings simulationSettings)
      {
         var chartTemplates = simulationSettings.ChartTemplates.ToList();
         if (!referencesOldName(chartTemplates))
            return;

         var newChartTemplates = new List<CurveChartTemplate>();
         foreach (var chartTemplate in simulationSettings.ChartTemplates)
         {
            var newChartTemplate = chartTemplate;
            if (referencesOldName(chartTemplate))
            {
               newChartTemplate = _cloneManager.Clone(chartTemplate);
               foreach (var curveTemplate in newChartTemplate.Curves)
               {
                  curveTemplate.xData.Path = stringReplace(curveTemplate.xData.Path);
                  curveTemplate.yData.Path = stringReplace(curveTemplate.yData.Path);
               }
            }

            newChartTemplates.Add(newChartTemplate);
         }

         _changes.Add(simulationSettings, _buildingBlock, new ReplaceBuildingBlockTemplatesCommand(simulationSettings, newChartTemplates));
      }

      private void checkOutputSelectionIn(SimulationSettings simulationSettings)
      {
         var outputSelections = simulationSettings.OutputSelections;
         if (!referencesOldName(outputSelections))
            return;

         var newOutputSelection = outputSelections.Clone();
         foreach (var outputUsingOldName in outputsReferencingOldName(newOutputSelection))
         {
            newOutputSelection.RemoveOutput(outputUsingOldName);
            newOutputSelection.AddOutput(new QuantitySelection(stringReplace(outputUsingOldName.Path), outputUsingOldName.QuantityType));
         }

         _changes.Add(outputSelections, _buildingBlock, new UpdateOutputSelectionInBuildingBlockCommand(newOutputSelection, simulationSettings));
      }

      private IReadOnlyList<QuantitySelection> outputsReferencingOldName(OutputSelections outputSelections)
      {
         return outputSelections.AllOutputs.Where(x => containsOldName(x.Path)).ToList();
      }

      private bool referencesOldName(CurveChartTemplate chartTemplate)
      {
         return chartTemplate.Curves.Any(c => containsOldName(c.xData.Path) || containsOldName(c.yData.Path));
      }

      private bool referencesOldName(IEnumerable<CurveChartTemplate> chartTemplates)
      {
         return chartTemplates.Any(referencesOldName);
      }

      private bool referencesOldName(OutputSelections outputSelections)
      {
         return outputsReferencingOldName(outputSelections).Any();
      }

      private void checkBuildingBlock<TBuildingBlock>(TBuildingBlock buildingBlock) where TBuildingBlock : IBuildingBlock
      {
         // We keep actual BB to provide Information to user
         _buildingBlock = buildingBlock;
         checkObjectBase(buildingBlock);
      }

      public void Visit(TransporterMoleculeContainer transporterMoleculeContainer)
      {
         checkObjectBase(transporterMoleculeContainer);
         if (!string.Equals(_oldName, transporterMoleculeContainer.TransportName)) return;

         _changes.Add(transporterMoleculeContainer, _buildingBlock,
            new EditObjectBasePropertyInBuildingBlockCommand(_transportNamePropertyName, _newName, _oldName, transporterMoleculeContainer, _buildingBlock),
            AppConstants.Commands.EditDescription(_objectTypeResolver.TypeFor<TransporterMoleculeContainer>(), _transportNamePropertyName, _oldName, _newName, transporterMoleculeContainer.Name));
      }

      public void Visit(InitialCondition initialCondition)
      {
         checkPathAndValueEntity(initialCondition, _msvPathTask);
      }

      public void Visit(ParameterValue parameterValue)
      {
         checkPathAndValueEntity(parameterValue, _parameterValuePathTask);
      }

      private void checkPathAndValueEntity<TPathAndValueEntity, TBuildingBlock>(TPathAndValueEntity pathAndValueEntity, IStartValuePathTask<TBuildingBlock, TPathAndValueEntity> startValueTask)
         where TPathAndValueEntity : PathAndValueEntity
         where TBuildingBlock : ILookupBuildingBlock<TPathAndValueEntity>
      {
         if (Equals(_objectToRename, pathAndValueEntity)) return;

         var entities = _buildingBlock as ILookupBuildingBlock<TPathAndValueEntity>;
         if (string.Equals(pathAndValueEntity.Name, _oldName))
            _changes.Add(pathAndValueEntity, _buildingBlock, startValueTask.UpdateNameCommand(entities, pathAndValueEntity, _newName));

         for (int i = 0; i < pathAndValueEntity.ContainerPath.Count; i++)
         {
            if (string.Equals(pathAndValueEntity.ContainerPath[i], _oldName))
               _changes.Add(pathAndValueEntity, _buildingBlock, startValueTask.UpdateContainerPathCommand(entities, pathAndValueEntity, i, _newName));
         }
      }

      public void Visit(ApplicationMoleculeBuilder applicationMoleculeBuilder)
      {
         checkObjectBase(applicationMoleculeBuilder);
         // possible path adjustments
         if (applicationMoleculeBuilder.RelativeContainerPath.Contains(_oldName))
         {
            _changes.Add(applicationMoleculeBuilder, _buildingBlock, new ChangePathElementAtContainerPathCommand(_newName, applicationMoleculeBuilder, _oldName, _buildingBlock));
         }
      }
   }
}