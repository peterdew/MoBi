﻿using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_HierarchicalSpatialStructurePresenter : ContextSpecification<HierarchicalSpatialStructurePresenter>
   {
      protected ITreeNodeFactory _treeNodeFactory;
      protected IViewItemContextMenuFactory _contextMenuFactory;
      protected IObjectBaseToObjectBaseDTOMapper _objectBaseToObjectBaseDTOMapper;
      protected IMoBiContext _context;
      protected IHierarchicalStructureView _view;
      protected MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         _view = A.Fake<IHierarchicalStructureView>();
         _context = A.Fake<IMoBiContext>();
         _objectBaseToObjectBaseDTOMapper = A.Fake<IObjectBaseToObjectBaseDTOMapper>();
         _contextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _treeNodeFactory = A.Fake<ITreeNodeFactory>();

         sut = new HierarchicalSpatialStructurePresenter(_view, _context, _objectBaseToObjectBaseDTOMapper, _contextMenuFactory, _treeNodeFactory);

         _spatialStructure = new MoBiSpatialStructure();
         sut.Edit(_spatialStructure);
      }
   }

   public class When_adding_a_non_root_node_to_the_spatial_structure : concern_for_HierarchicalSpatialStructurePresenter
   {
      private IContainer _addedObject;
      private ObjectBaseDTO _dto;
      private Container _rootContainer;
      private ObjectBaseDTO _rootDTO;

      protected override void Context()
      {
         base.Context();
         _addedObject = new Container();
         _rootContainer = new Container {_addedObject};
         _dto = new ObjectBaseDTO();
         _rootDTO = new ObjectBaseDTO();
         _spatialStructure.Add(_rootContainer);

         A.CallTo(() => _objectBaseToObjectBaseDTOMapper.MapFrom(_addedObject)).Returns(_dto);
         A.CallTo(() => _objectBaseToObjectBaseDTOMapper.MapFrom(_rootContainer)).Returns(_rootDTO);
      }

      protected override void Because()
      {
         sut.Handle(new AddedEvent<IContainer>(_addedObject, _spatialStructure));
      }

      [Observation]
      public void the_new_container_should_be_added_below_the_parent_in_the_tree_view()
      {
         A.CallTo(() => _view.Add(_dto, _rootDTO)).MustHaveHappened();
      }

      public class When_adding_a_root_node_to_the_spatial_structure : concern_for_HierarchicalSpatialStructurePresenter
      {
         private IContainer _addedObject;
         private ObjectBaseDTO _dto;

         protected override void Context()
         {
            base.Context();
            _addedObject = new Container();
            _spatialStructure.AddTopContainer(_addedObject);
            _dto = new ObjectBaseDTO();

            A.CallTo(() => _objectBaseToObjectBaseDTOMapper.MapFrom(_addedObject)).Returns(_dto);
         }

         protected override void Because()
         {
            sut.Handle(new AddedEvent<IContainer>(_addedObject, _spatialStructure));
         }

         [Observation]
         public void the_new_container_should_be_added_to_the_root_of_the_tree_view()
         {
            A.CallTo(() => _view.AddRoot(_dto)).MustHaveHappened();
         }
      }

      public class When_adding_a_neighborhood_to_the_spatial_structure : concern_for_HierarchicalSpatialStructurePresenter
      {
         private NeighborhoodBuilder _addedNeighborhood;
         private ObjectBaseDTO _dto;
         private ObjectBaseDTO _neighborhoodsContainerDTO;

         protected override void Context()
         {
            base.Context();
            _spatialStructure.NeighborhoodsContainer = new Container().WithName("NeighborhoodsContainer");
            _addedNeighborhood = new NeighborhoodBuilder().WithName("Neighborhood").Under(_spatialStructure.NeighborhoodsContainer);
            _dto = new ObjectBaseDTO();
            _neighborhoodsContainerDTO = new ObjectBaseDTO();

            A.CallTo(() => _objectBaseToObjectBaseDTOMapper.MapFrom(_spatialStructure.NeighborhoodsContainer)).Returns(_neighborhoodsContainerDTO);
            A.CallTo(() => _objectBaseToObjectBaseDTOMapper.MapFrom(_addedNeighborhood)).Returns(_dto);
         }

         protected override void Because()
         {
            sut.Handle(new AddedEvent<IContainer>(_addedNeighborhood, _spatialStructure));
         }

         [Observation]
         public void the_new_container_should_be_added_to_the_root_of_the_tree_view()
         {
            A.CallTo(() => _view.Add(_dto, _neighborhoodsContainerDTO)).MustHaveHappened();
         }
      }
   }
}