﻿using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForSimulation : ContextMenuBase, IContextMenuFor<IMoBiSimulation>
   {
      private readonly IMoBiContext _context;
      private readonly IContainer _container;
      private readonly IInteractionTasksForSimulation _interactionTask;
      private List<IMenuBarItem> _allMenuItems;

      public ContextMenuForSimulation(IMoBiContext context, IContainer container, IInteractionTasksForSimulation interactionTask)
      {
         _context = context;
         _container = container;
         _interactionTask = interactionTask;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var simulation = dto.DowncastTo<SimulationViewItem>().Simulation;

         //a simulation might not be registered yet which is required to execute some of the action
         _context.Register(simulation);
         _allMenuItems = new List<IMenuBarItem>
         {
            createEditItem(simulation),
            createRenameItem(simulation),
            createConfigure(simulation).AsGroupStarter()
         };


         if (simulation.BuildingBlocks().Any(hasChanges))
            _allMenuItems.Add(createUpdate(simulation));

         if (simulation.OriginalQuantityValues.Any())
            _allMenuItems.Add(createCommit(simulation));

         _allMenuItems.AddRange(new[]
         {
            createRunItem(simulation).AsGroupStarter(),
            createParameterIdentificationItem(simulation),
            createSensitivityAnalysisItem(simulation),

            createStartPopulationSimulation(simulation).AsGroupStarter(),
            createSaveItem(simulation),
            createAddToJournal(simulation),
            exportSimulationResultsToExcel(simulation),
            createExportODEForMatlabItem(simulation),
            createExportODEForRItem(simulation),
            createExportForSimModelXmlItem(simulation),
            createExportForCppItem(simulation),
            createDebugReportItem(simulation),
            createExportModelPartsItem(simulation),

            createImportReactionParameters(simulation).AsGroupStarter(),
            createClone(simulation).AsGroupStarter(),
            createDeleteItem(simulation).AsGroupStarter(),
            createDeleteAllResultsItem(simulation)
         });

         return this;
      }

      private bool hasChanges(IBuildingBlock simulationBuildingBlock)
      {
         return _interactionTask.TemplateBuildingBlockFor(simulationBuildingBlock).Version != simulationBuildingBlock.Version;
      }

      private IMenuBarItem createSensitivityAnalysisItem(IMoBiSimulation simulation)
      {
         return SensitivityAnalysisContextMenuItems.CreateSensitivityAnalysisFor(simulation, _container);
      }

      private IMenuBarItem createImportReactionParameters(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.ImportSimulationParameters.WithEllipsis())
            .WithIcon(ApplicationIcons.ParameterValuesImport)
            .WithCommandFor<ImportSimulationParameterValuesUICommand, IMoBiSimulation>(simulation, _container);
      }

      private IMenuBarItem createClone(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Clone)
            .WithIcon(ApplicationIcons.SimulationClone)
            .WithCommandFor<CloneSimulationUICommand, IMoBiSimulation>(simulation, _container);
      }

      private IMenuBarItem createExportModelPartsItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.ExportModelAsTables)
            .WithIcon(ApplicationIcons.ObservedData)
            .WithCommandFor<ExportModelPartsToExcelUICommand, IMoBiSimulation>(simulation, _container);
      }

      private IMenuBarItem createRunItem(IMoBiSimulation simulation)
      {
         return
            CreateMenuButton.WithCaption(AppConstants.MenuNames.Run)
               .WithIcon(ApplicationIcons.Run)
               .WithCommandFor<RunSimulationCommand, IMoBiSimulation>(simulation, _container);
      }

      private IMenuBarItem createExportODEForMatlabItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(MenuNames.ExportODEForMatlab)
            .WithCommandFor<ExportODEForMatlabUICommand, IMoBiSimulation>(simulation, _container)
            .WithIcon(ApplicationIcons.Matlab);
      }

      private IMenuBarItem createExportODEForRItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(MenuNames.AsDeveloperOnly(MenuNames.ExportODEForR))
            .WithCommandFor<ExportODEForRUICommand, IMoBiSimulation>(simulation, _container)
            .WithIcon(ApplicationIcons.R)
            .ForDeveloper();
      }

      private IMenuBarItem createParameterIdentificationItem(IMoBiSimulation simulation)
      {
         return ParameterIdentificationContextMenuItems.CreateParameterIdentificationFor(new[] { simulation }, _container);
      }

      private IMenuBarItem createDebugReportItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.SimulationReport)
            .WithIcon(ApplicationIcons.Report)
            .WithCommandFor<CreateSimulationReportUICommand, IMoBiSimulation>(simulation, _container);
      }

      private IMenuBarItem createExportForSimModelXmlItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(MenuNames.ExportSimModelXml)
            .WithCommandFor<ExportSimulationToSimModelXmlUICommand, IMoBiSimulation>(simulation, _container)
            .ForDeveloper();
      }

      private IMenuBarItem createExportForCppItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(MenuNames.ExportForCpp)
            .WithCommandFor<ExportSimulationToCppUICommand, IMoBiSimulation>(simulation, _container)
            .ForDeveloper();
      }

      private IMenuBarItem createStartPopulationSimulation(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.StartPopulationSimulation)
            .WithIcon(ApplicationIcons.PopulationSimulation)
            .WithCommandFor<SendSimulationToPKSimUICommand, IMoBiSimulation>(simulation, _container);
      }

      private IMenuBarItem createSaveItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveToPkmlFormat)
            .WithCommandFor<SaveUICommandFor<IMoBiSimulation>, IMoBiSimulation>(simulation, _container)
            .WithIcon(ApplicationIcons.PKMLSave);
      }

      private IMenuBarItem createDeleteItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithCommandFor<RemoveSimulationUICommand, IMoBiSimulation>(simulation, _container)
            .WithIcon(ApplicationIcons.Delete);
      }

      private IMenuBarItem createDeleteAllResultsItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.DeleteAllResults)
            .WithCommandFor<DeleteAllResultsInSimulationUICommand, IMoBiSimulation>(simulation, _container);
      }

      private IMenuBarItem createRenameItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
            .WithCommandFor<RenameSimulationUICommand, IMoBiSimulation>(simulation, _container).WithIcon(ApplicationIcons.Rename);
      }

      private IMenuBarItem createEditItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Edit)
            .WithIcon(ApplicationIcons.Edit)
            .WithCommandFor<EditSimulationUICommand, IMoBiSimulation>(simulation, _container);
      }

      private IMenuBarItem createCommit(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.CommitToBuildingBlocks)
            .WithIcon(ApplicationIcons.Commit)
            .WithCommandFor<CommitSimulationUICommand, IMoBiSimulation>(simulation, _container);
      }

      private IMenuBarItem createUpdate(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.UpdateFromBuildingBlocks)
            .WithIcon(ApplicationIcons.Update)
            .WithCommandFor<UpdateSimulationUICommand, IMoBiSimulation>(simulation, _container);
      }

      private IMenuBarItem createConfigure(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Configure)
            .WithIcon(ApplicationIcons.SimulationConfigure)
            .WithCommandFor<ConfigureSimulationUICommand, IMoBiSimulation>(simulation, _container);
      }


      private IMenuBarItem exportSimulationResultsToExcel(IMoBiSimulation simulation)
      {
         //create sub menu containing all compounds
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.ExportSimulationResultsToExcel)
            .WithCommandFor<ExportSimulationResultsToExcelCommand, IMoBiSimulation>(simulation, _container)
            .WithIcon(ApplicationIcons.ObservedData);
      }

      private IMenuBarItem createAddToJournal(IMoBiSimulation simulation)
      {
         return ObjectBaseCommonContextMenuItems.AddToJournal(simulation, _container);
      }
   }

   internal class ContextMenuSpecificationFactoryForSimulation : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IMoBiContext _context;
      private readonly IContainer _container;
      private readonly IInteractionTasksForSimulation _interactionTasks;

      public ContextMenuSpecificationFactoryForSimulation(IMoBiContext context, IContainer container, IInteractionTasksForSimulation interactionTasks)
      {
         _context = context;
         _container = container;
         _interactionTasks = interactionTasks;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new ContextMenuForSimulation(_context, _container, _interactionTasks).InitializeWith(viewItem.DowncastTo<SimulationViewItem>(), presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<SimulationViewItem>();
      }
   }
}