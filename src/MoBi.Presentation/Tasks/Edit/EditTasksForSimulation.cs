using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTasksForSimulation : IEditTaskFor<IMoBiSimulation>
   {
      void CreateReport(IModelCoreSimulation simulation);
      void ExportResultsToExcel(IMoBiSimulation simulation);
      void ExportResultsToExcel(IMoBiSimulation simulation, DataRepository dataRepository);
      void RenameResults(IMoBiSimulation simulation, DataRepository dataRepository);
      void ExportODEForMatlab(IMoBiSimulation simulation);
      void ExportODEForR(IMoBiSimulation simulation);
      void ExportSimulationToCppCode(IMoBiSimulation simulation);
      void ExportSimModelXml(IMoBiSimulation simulation);
      void CalculateScaleFactors(IMoBiSimulation simulation);
   }

   public class EditTasksForSimulation : EditTasksForBuildingBlock<IMoBiSimulation>, IEditTasksForSimulation
   {
      private readonly ISimulationPersistor _simulationPersistor;
      private readonly IDialogCreator _dialogCreator;
      private readonly IModelReportCreator _reportCreator;
      private readonly IDataRepositoryExportTask _dataRepositoryTask;
      private readonly ISimModelExporter _simModelExporter;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IParameterIdentificationSimulationPathUpdater _parameterIdentificationSimulationPathUpdater;

      public EditTasksForSimulation(
         IInteractionTaskContext interactionTaskContext,
         ISimulationPersistor simulationPersistor,
         IDialogCreator dialogCreator,
         IDataRepositoryExportTask dataRepositoryTask,
         IModelReportCreator reportCreator,
         ISimModelExporter simModelExporter,
         IDimensionFactory dimensionFactory,
         IParameterIdentificationSimulationPathUpdater parameterIdentificationSimulationPathUpdater) : base(interactionTaskContext)
      {
         _simulationPersistor = simulationPersistor;
         _dialogCreator = dialogCreator;
         _dataRepositoryTask = dataRepositoryTask;
         _reportCreator = reportCreator;
         _simModelExporter = simModelExporter;
         _dimensionFactory = dimensionFactory;
         _parameterIdentificationSimulationPathUpdater = parameterIdentificationSimulationPathUpdater;
      }

      public void CreateReport(IModelCoreSimulation simulation)
      {
         var exportFile = _interactionTask.AskForFileToSave(AppConstants.Dialog.ExportSimulationModelToFileTitle, Constants.Filter.TEXT_FILE_FILTER, Constants.DirectoryKey.REPORT, simulation.Name);
         if (exportFile.IsNullOrEmpty()) return;
         using (var writer = new StreamWriter(exportFile))
         {
            writer.Write(_reportCreator.ModelReport(simulation.Model, true, true, true));
         }

         //now try to open the file
         FileHelper.TryOpenFile(exportFile);
      }

      public override void Edit(IMoBiSimulation simulation)
      {
         //simulation are not registered in repository anymore. We need to do it
         _interactionTaskContext.Context.Register(simulation);
         base.Edit(simulation);
      }

      public void ExportResultsToExcel(IMoBiSimulation simulation)
      {
         ExportResultsToExcel(simulation, simulation.ResultsDataRepository);
      }

      public void ExportResultsToExcel(IMoBiSimulation simulation, DataRepository dataRepository)
      {
         if (dataRepository == null) return;

         exportAllResults(simulation, dataRepository);
      }

      private void exportAllResults(IMoBiSimulation simulation, DataRepository dataRepository)
      {
         var fileName = _interactionTask.AskForFileToSave(AppConstants.Dialog.ExportSimulationResultsToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.REPORT, simulation.Name);
         if (string.IsNullOrEmpty(fileName)) return;

         exportDataRepository(fileName, dataRepository);
      }

      private void exportDataRepository(string fileName, IEnumerable<DataColumn> dataRepository)
      {
         var dataTables = _dataRepositoryTask.ToDataTable(dataRepository, new DataColumnExportOptions
         {
            ColumnNameRetriever = x => x.QuantityInfo.PathAsString,
            DimensionRetriever = _dimensionFactory.MergedDimensionFor
         });

         _dataRepositoryTask.ExportToExcel(dataTables, fileName, true);
      }

      public void RenameResults(IMoBiSimulation simulation, DataRepository dataRepository)
      {
         var newName = _interactionTaskContext.NamingTask.RenameFor(dataRepository, allUsedResultsNameIn(simulation));

         if (string.IsNullOrEmpty(newName))
            return;

         addCommand(new RenameSimulationResultsCommand(dataRepository, simulation, newName).Run(_context));
      }

      public void ExportODEForMatlab(IMoBiSimulation simulation)
      {
         var exportFolder = _interactionTask.AskForFolder(AppConstants.Dialog.ExportODEForMatlab, Constants.DirectoryKey.SIM_MODEL_XML);
         if (string.IsNullOrEmpty(exportFolder)) return;
         _simModelExporter.ExportODEForMatlab(simulation, exportFolder, FormulaExportMode.Formula);
      }

      public void ExportODEForR(IMoBiSimulation simulation)
      {
         var exportFolder = _interactionTask.AskForFolder(AppConstants.Dialog.ExportODEForR, Constants.DirectoryKey.SIM_MODEL_XML);
         if (string.IsNullOrEmpty(exportFolder)) return;
         _simModelExporter.ExportODEForR(simulation, exportFolder, FormulaExportMode.Formula);
      }

      public void ExportSimulationToCppCode(IMoBiSimulation simulation)
      {
         var exportFolder = _interactionTask.AskForFolder(AppConstants.Dialog.ExportSimulationToCppCode, Constants.DirectoryKey.SIM_MODEL_XML);
         if (string.IsNullOrEmpty(exportFolder)) return;
         _simModelExporter.ExportCppCode(simulation, exportFolder, FormulaExportMode.Formula);
      }

      public void CalculateScaleFactors(IMoBiSimulation simulation)
      {
         using (var presenter = _applicationController.Start<ICalculateScaleDivisorsPresenter>())
         {
            var command = presenter.CalculateScaleDivisorFor(simulation);
            if (command.IsEmpty())
               return;

            addCommand(command);
         }
      }

      private IReadOnlyList<string> allUsedResultsNameIn(IMoBiSimulation simulation)
      {
         return simulation.HistoricResults.Select(x => x.Name).Union(new[] { simulation.ResultsDataRepository.Name }).ToList();
      }

      private void addCommand(IMoBiCommand command)
      {
         _context.AddToHistory(command);
      }

      protected override IEnumerable<string> GetUnallowedNames(IMoBiSimulation simulation, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         return _context.CurrentProject.Simulations.Select(bb => bb.Name);
      }

      public override void Save(IMoBiSimulation simulation)
      {
         var fileName = _dialogCreator.AskForFileToSave(AppConstants.Captions.Save, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, simulation.Name);
         if (fileName.IsNullOrEmpty()) return;
         _simulationPersistor.Save(new SimulationTransfer { Simulation = simulation }, fileName);
      }

      public void ExportSimModelXml(IMoBiSimulation simulation)
      {
         var fileName = _dialogCreator.AskForFileToSave(AppConstants.Captions.Save, AppConstants.Filter.SIM_MODEL_FILE_FILTER, Constants.DirectoryKey.SIM_MODEL_XML, simulation.Name);
         if (fileName.IsNullOrEmpty()) return;
         _simModelExporter.ExportSimModelXml(simulation, fileName);
      }

      public override void Rename(IMoBiSimulation simulationToRename, IEnumerable<IObjectBase> existingObjectsInParent, IBuildingBlock buildingBlock)
      {
         string oldName = simulationToRename.Name;
         base.Rename(simulationToRename, existingObjectsInParent, buildingBlock);

         //It was not renamed after all
         if (string.Equals(oldName, simulationToRename.Name))
            return;

         _parameterIdentificationSimulationPathUpdater.UpdatePathsForRenamedSimulation(simulationToRename, oldName, simulationToRename.Name);
         simulationToRename.HasChanged = true;
      }
   }
}