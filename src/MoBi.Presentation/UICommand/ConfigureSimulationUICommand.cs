﻿using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   public class ConfigureSimulationUICommand : ReconfigureSimulationUICommand
   {
      public ConfigureSimulationUICommand(
         ISimulationUpdateTask simulationUpdateTask,
         IMoBiContext context,
         IActiveSubjectRetriever activeSubjectRetriever) :
         base(activeSubjectRetriever, simulationUpdateTask, context)
      {
      }

      protected override ICommand PerformReconfigure()
      {
         return _simulationUpdateTask.ConfigureSimulation(Subject);
      }
   }
}