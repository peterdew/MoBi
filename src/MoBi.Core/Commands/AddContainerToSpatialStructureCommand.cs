using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public class AddContainerToSpatialStructureCommand : AddObjectBaseCommand<IContainer, IContainer>
   {
      private MoBiSpatialStructure _spatialStructure;
      public string SpatialStructureId { get; set; }

      public AddContainerToSpatialStructureCommand(IContainer parent, IContainer child, MoBiSpatialStructure spatialStructure) : base(parent, child, spatialStructure)
      {
         _spatialStructure = spatialStructure;
         SpatialStructureId = spatialStructure.Id;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _spatialStructure = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _spatialStructure = context.Get<MoBiSpatialStructure>(SpatialStructureId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveContainerFromSpatialStructureCommand(_parent, _itemToAdd, _spatialStructure).AsInverseFor(this);
      }

      protected override void AddTo(IContainer child, IContainer parent, IMoBiContext context)
      {
         parent.Add(child);
         _spatialStructure.DiagramManager.AddObjectBase(child);
      }
   }
}