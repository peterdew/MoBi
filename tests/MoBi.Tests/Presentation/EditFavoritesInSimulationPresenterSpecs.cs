﻿using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditFavoritesInSimulationPresenter : ContextSpecification<EditFavoritesInSimulationPresenter>
   {
      protected IEntityPathResolver _entityPathResolver;
      protected IFavoriteRepository _favoriteRepository;
      protected IEditFavoritesView _view;
      protected IEditParameterListPresenter _editParameterListPresenter;
      private IFavoriteTask _favoriteTask;

      protected override void Context()
      {
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         _favoriteRepository = A.Fake<IFavoriteRepository>();
         _view = A.Fake<IEditFavoritesView>();
         _editParameterListPresenter = A.Fake<IEditParameterListPresenter>();
         _favoriteTask = A.Fake<IFavoriteTask>();
         sut = new EditFavoritesInSimulationPresenter(_view, _favoriteRepository, _entityPathResolver, _editParameterListPresenter, _favoriteTask);
      }
   }

   public class When_editing_a_simulation : concern_for_EditFavoritesInSimulationPresenter
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
      }

      protected override void Because()
      {
         sut.Edit(_simulation);
      }

      [Observation]
      public void the_simulation_should_be_cached()
      {
         A.CallTo(() => _editParameterListPresenter.SetVisibility(PathElementId.Simulation, false)).MustHaveHappened();
      }
   }

   public class When_handling_a_quantity_changed_event_and_the_quantity_is_a_favorite : concern_for_EditFavoritesInSimulationPresenter
   {
      private IParameter _quantity;
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _quantity = new Parameter();
         A.CallTo(() => _favoriteRepository.All()).Returns(new[] { string.Empty });

         _simulation = new MoBiSimulation
         {
            Configuration = new SimulationConfiguration(),
            Model = new Model
            {
               Root = new Container()
            }
         };
         _simulation.Model.Root.Add(_quantity);
         sut.Edit(_simulation);
      }

      protected override void Because()
      {
         sut.Handle(new AddParameterToFavoritesEvent(""));
      }

      [Observation]
      public void the_view_should_be_rebound()
      {
         A.CallTo(() => _editParameterListPresenter.Edit(A<IEnumerable<IParameter>>._)).MustHaveHappened();
      }
   }
}