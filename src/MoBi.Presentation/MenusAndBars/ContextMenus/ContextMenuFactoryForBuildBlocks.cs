﻿using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuFactoryForBuildingBlock<TBuildingBlock> : IContextMenuSpecificationFactory<IViewItem> where TBuildingBlock : IBuildingBlock
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = IoC.Resolve<IContextMenuForBuildingBlock<TBuildingBlock>>();
         return contextMenu.InitializeWith(viewItem.DowncastTo<BuildingBlockViewItem>(), presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var buildingBlockViewItem = viewItem as BuildingBlockViewItem;
         if (buildingBlockViewItem == null) return false;
         return buildingBlockViewItem.BuildingBlock.IsAnImplementationOf<TBuildingBlock>();
      }
   }

   public abstract class RootNodeContextMenuFactoryFor<TObjectBase> : IContextMenuSpecificationFactory<IViewItem> where TObjectBase : IObjectBase
   {
      private readonly RootNodeType _rootNodeType;

      protected RootNodeContextMenuFactoryFor(RootNodeType rootNodeType)
      {
         _rootNodeType = rootNodeType;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return IoC.Resolve<IRootContextMenuFor<MoBiProject, TObjectBase>>().InitializeWith(_rootNodeType, presenter.DowncastTo<IExplorerPresenter>());
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var rootNodeType = viewItem as RootNodeType;
         return Equals(_rootNodeType, rootNodeType) && presenter.IsAnImplementationOf<IExplorerPresenter>();
      }
   }

   public abstract class RootContextMenuFactoryFor<TParent, TChild> : IContextMenuSpecificationFactory<IViewItem> where TChild : IObjectBase
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return IoC.Resolve<IRootContextMenuFor<TParent, TChild>>().InitializeWith(presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<IRootViewItem<TChild>>();
      }
   }

   public class RootContextMenuFactoryForIndividualBuildingBlock : RootNodeContextMenuFactoryFor<IndividualBuildingBlock>
   {
      public RootContextMenuFactoryForIndividualBuildingBlock() : base(MoBiRootNodeTypes.IndividualsFolder)
      {
      }
   }

   public class RootContextMenuFactoryForExtensionModule : RootNodeContextMenuFactoryFor<Module>
   {
      public RootContextMenuFactoryForExtensionModule() : base(MoBiRootNodeTypes.ModulesFolder)
      {
      }
   }

   public class RootContextMenuFactoryForExpressionProfileBuildingBlock : RootNodeContextMenuFactoryFor<ExpressionProfileBuildingBlock>
   {
      public RootContextMenuFactoryForExpressionProfileBuildingBlock() : base(MoBiRootNodeTypes.ExpressionProfilesFolder)
      {
      }
   }

   public class RootContextMenuFactoryForSimulation : RootNodeContextMenuFactoryFor<IMoBiSimulation>
   {
      public RootContextMenuFactoryForSimulation() : base(RootNodeTypes.SimulationFolder)
      {
      }
   }

   public class RootContextMenuFactoryForParameterIdentification : RootNodeContextMenuFactoryFor<ParameterIdentification>
   {
      public RootContextMenuFactoryForParameterIdentification() : base(RootNodeTypes.ParameterIdentificationFolder)
      {
      }
   }

   public class RootContextMenuFactoryForSensitivityAnalysis : RootNodeContextMenuFactoryFor<SensitivityAnalysis>
   {
      public RootContextMenuFactoryForSensitivityAnalysis() : base(RootNodeTypes.SensitivityAnalysisFolder)
      {
      }
   }

   public class RootContextMenuFactoryForReactionBuilder : RootContextMenuFactoryFor<MoBiReactionBuildingBlock, ReactionBuilder>
   {
   }

   public class RootContextMenuFactoryForTransportBuilder : RootContextMenuFactoryFor<PassiveTransportBuildingBlock, TransportBuilder>
   {
   }

   public class RootContextMenuFactoryForAmountObserverBuilder : RootContextMenuFactoryFor<ObserverBuildingBlock, AmountObserverBuilder>
   {
   }

   public class RootContextMenuFactoryForContainerObserverBuilder : RootContextMenuFactoryFor<ObserverBuildingBlock, ContainerObserverBuilder>
   {
   }
}