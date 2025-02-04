﻿using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_EditTasksForApplicationBuilderSpecs : ContextSpecification<IEditTaskFor<ApplicationBuilder>>
   {
      protected const string _builderName = "Name";
      protected IInteractionTaskContext _interactionTaskContext;
      private IInteractionTask _interactionTask;
      protected ApplicationBuilder _builder;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _interactionTask = A.Fake<IInteractionTask>();
         A.CallTo(() => _interactionTaskContext.InteractionTask).Returns(_interactionTask);
         _builder = new ApplicationBuilder();
         sut = new EditTasksForEventGroupBuilder<ApplicationBuilder>(_interactionTaskContext);

         A.CallTo(() => _interactionTask.ForbiddenNamesFor(_builder)).Returns(new List<string> {_builderName});
      }
   }
   
   internal class When_asking_for_forbidden_names_without_self_with_like_named_entity : concern_for_EditTasksForApplicationBuilderSpecs
   {
      private IEnumerable<string> _result;
      private Container _parentContainer;

      protected override void Context()
      {
         base.Context();

         _parentContainer = new Container { _builder, new ApplicationBuilder { Name = _builderName } };
         _builder.Name = _builderName;
      }

      protected override void Because()
      {
         _result = sut.GetForbiddenNamesWithoutSelf(_builder, _parentContainer);
      }

      [Observation]
      public void should_not_return_builder_name_as_forbidden()
      {
         _result.ShouldNotContain(_builderName);
      }
   }

   internal class When_asking_for_forbidden_names_without_self_only_self_added : concern_for_EditTasksForApplicationBuilderSpecs
   {
      private IContainer _parentContainer;
      private IEnumerable<string> _result;

      protected override void Context()
      {
         base.Context();
         _parentContainer = new Container();
         _parentContainer.Add(_builder);
         _builder.Name = "Name";
      }

      protected override void Because()
      {
         _result = sut.GetForbiddenNamesWithoutSelf(_builder, Enumerable.Empty<IObjectBase>());
      }

      [Observation]
      public void should_not_return_builder_name_as_forbidden()
      {
         _result.ShouldBeEmpty();
      }
   }

   internal class When_asking_for_forbidden_Names : concern_for_EditTasksForApplicationBuilderSpecs
   {
      private IEnumerable<string> _forbiddenNames;
      private string[] _allNames;
      private EventGroupBuildingBlock _eventGroupBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _eventGroupBuildingBlock = new EventGroupBuildingBlock().WithName("Tada");
         _eventGroupBuildingBlock.Add(new ApplicationBuilder().WithName("App1"));
         _eventGroupBuildingBlock.Add(new ApplicationBuilder().WithName("App2"));
         _eventGroupBuildingBlock.Add(new EventGroupBuilder().WithName("EG1"));
         _allNames = _eventGroupBuildingBlock.Select(x => x.Name).ToArray();

         A.CallTo(() => _interactionTaskContext.Active<EventGroupBuildingBlock>()).Returns(_eventGroupBuildingBlock);
      }

      protected override void Because()
      {
         _forbiddenNames = sut.GetForbiddenNamesWithoutSelf(new ApplicationBuilder(), _eventGroupBuildingBlock);
      }

      [Observation]
      public void should_return_all_root_names_of_the_event_group_building_block()
      {
         _forbiddenNames.ShouldContain(_allNames);
      }
   }
}