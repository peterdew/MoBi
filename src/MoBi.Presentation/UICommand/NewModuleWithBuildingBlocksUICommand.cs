using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.UICommand
{
   public class NewModuleWithBuildingBlocksUICommand : IUICommand
   {
      private readonly IInteractionTasksForModule _interactionTasks;

      public NewModuleWithBuildingBlocksUICommand(IInteractionTasksForModule interactionTasksForModule)
      {
         _interactionTasks = interactionTasksForModule;
      }

      public void Execute()
      {
         _interactionTasks.CreateNewModuleWithBuildingBlocks();
      }
   }
}