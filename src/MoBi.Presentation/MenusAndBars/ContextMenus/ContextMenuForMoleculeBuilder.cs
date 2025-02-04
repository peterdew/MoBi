﻿using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuSpecificationFactoryForMoleculeBuilder : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return IoC.Resolve<IContextMenuForMoleculeBuilder>().InitializeWith(viewItem as MoleculeBuilderDTO, presenter);
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return (objectRequestingContextMenu == null && presenter.IsAnImplementationOf<IMoleculeListPresenter>()) ||
                objectRequestingContextMenu.IsAnImplementationOf<MoleculeBuilderDTO>();
      }
   }

   public interface IContextMenuForMoleculeBuilder : IContextMenuFor<MoleculeBuilder>
   {
   }

   public class ContextMenuForMoleculeBuilder : ContextMenuBase, IContextMenuForMoleculeBuilder
   {
      private IList<IMenuBarItem> _allMolecules;
      private readonly IMoBiContext _context;
      private readonly IContainer _container;

      public ContextMenuForMoleculeBuilder(IMoBiContext context, IContainer container)
      {
         _context = context;
         _container = container;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMolecules;
      }

      public IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var listPresenter = presenter.DowncastTo<IMoleculeListPresenter>();
         var moleculeBuildingBlock = listPresenter.MoleculeBuildingBlock;
         if (dto == null)
         {
            _allMolecules = new List<IMenuBarItem>
            {
               createAddNewMoleculeBuilder(moleculeBuildingBlock),
               createAddExistingMoleculeBuilder(moleculeBuildingBlock),
               createAddExistingMoleculeBuilderFromTemplate(moleculeBuildingBlock),
               createAddPKSimMoleculeFromTemplate(moleculeBuildingBlock),
            };
            return this;
         }

         var moleculeBuilder = _context.Get<MoleculeBuilder>(dto.Id);
         _allMolecules = new List<IMenuBarItem>
         {
            createEditItemFor(moleculeBuilder),
            createRenameItemFor(moleculeBuilder),
            createAddNewTransporterFor(moleculeBuilder),
            createAddExistingTransporterFor(moleculeBuilder),
            createAddExistingFromTemplateTransporterFor(moleculeBuilder),
            createAddNewInteractionContainerFor(moleculeBuilder),
            createAddExistingInteractionContainerFor(moleculeBuilder),
            createAddExistingFromTemplateInteractionContainerFor(moleculeBuilder),
            createSaveItemFor(moleculeBuilder),
            createRemoveItemFor(moleculeBuildingBlock, moleculeBuilder)
         };

         return this;
      }

      private IMenuBarItem createAddNewInteractionContainerFor(MoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.InteractionContainer))
            .WithCommandFor<AddNewCommandFor<MoleculeBuilder, InteractionContainer>, MoleculeBuilder>(moleculeBuilder, _container)
            .WithIcon(ApplicationIcons.Add);
      }
      
      private IMenuBarItem createAddExistingInteractionContainerFor(MoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(
            AppConstants.MenuNames.AddExisting(ObjectTypes.InteractionContainer))
            .WithCommandFor<AddExistingCommandFor<MoleculeBuilder, InteractionContainer>, MoleculeBuilder>(moleculeBuilder, _container)
            .WithIcon(ApplicationIcons.PKMLLoad);
      }

      private IMenuBarItem createAddExistingFromTemplateInteractionContainerFor(MoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate(ObjectTypes.InteractionContainer))
            .WithCommandFor<AddExistingFromTemplateCommandFor<MoleculeBuilder, InteractionContainer>, MoleculeBuilder>(moleculeBuilder, _container)
            .WithIcon(ApplicationIcons.LoadFromTemplate);
      }
      private IMenuBarItem createEditItemFor(MoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Edit)
            .WithCommandFor<EditCommandFor<MoleculeBuilder>, MoleculeBuilder>(moleculeBuilder, _container)
            .WithIcon(ApplicationIcons.Edit);
      }

      private IMenuBarItem createRenameItemFor(MoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
            .WithCommandFor<RenameObjectCommand<MoleculeBuilder>, MoleculeBuilder>(moleculeBuilder, _container)
            .WithIcon(ApplicationIcons.Rename);
      }

      private IMenuBarItem createRemoveItemFor(MoleculeBuildingBlock moleculeBuildingBlock, MoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithRemoveCommand(moleculeBuildingBlock, moleculeBuilder)
            .WithIcon(ApplicationIcons.Delete);
      }

      private IMenuBarItem createAddNewTransporterFor(MoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.TransporterMoleculeContainer))
            .WithCommandFor<AddNewCommandFor<MoleculeBuilder, TransporterMoleculeContainer>, MoleculeBuilder>(moleculeBuilder, _container)
            .WithIcon(ApplicationIcons.Add);
      }

      private IMenuBarItem createAddExistingTransporterFor(MoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.TransporterMoleculeContainer))
            .WithCommandFor<AddExistingCommandFor<MoleculeBuilder, TransporterMoleculeContainer>, MoleculeBuilder>(moleculeBuilder, _container)
            .WithIcon(ApplicationIcons.PKMLLoad);
      }

      private IMenuBarItem createAddExistingFromTemplateTransporterFor(MoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate(ObjectTypes.TransporterMoleculeContainer))
            .WithCommandFor<AddExistingFromTemplateCommandFor<MoleculeBuilder, TransporterMoleculeContainer>, MoleculeBuilder>(moleculeBuilder, _container)
            .WithIcon(ApplicationIcons.LoadFromTemplate);
      }
      
      private IMenuBarItem createAddNewMoleculeBuilder(MoleculeBuildingBlock moleculeBuildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.Molecule))
            .WithCommandFor<AddNewCommandFor<MoleculeBuildingBlock, MoleculeBuilder>, MoleculeBuildingBlock>(moleculeBuildingBlock, _container)
            .WithIcon(ApplicationIcons.MoleculeAdd);
      }

      private IMenuBarItem createAddPKSimMoleculeFromTemplate(MoleculeBuildingBlock moleculeBuildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddPKSimMolecule)
            .WithCommandFor<AddPKSimMoleculeCommand, MoleculeBuildingBlock>(moleculeBuildingBlock, _container)
            .WithIcon(ApplicationIcons.PKSimMoleculeAdd);
      }

      private IMenuBarItem createAddExistingMoleculeBuilder(MoleculeBuildingBlock moleculeBuildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.Molecule))
            .WithCommandFor<AddExistingCommandFor<MoleculeBuildingBlock, MoleculeBuilder>, MoleculeBuildingBlock>(moleculeBuildingBlock, _container)
            .WithIcon(ApplicationIcons.MoleculeLoad);
      }

      private IMenuBarItem createAddExistingMoleculeBuilderFromTemplate(MoleculeBuildingBlock moleculeBuildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate(ObjectTypes.Molecule))
            .WithCommandFor<AddExistingFromTemplateCommandFor<MoleculeBuildingBlock, MoleculeBuilder>, MoleculeBuildingBlock>(moleculeBuildingBlock, _container)
            .WithIcon(ApplicationIcons.LoadFromTemplate);
      }

      private IMenuBarItem createSaveItemFor(MoleculeBuilder selectedObject)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveAsPKML)
            .WithCommandFor<SaveUICommandFor<MoleculeBuilder>, MoleculeBuilder>(selectedObject, _container)
            .WithIcon(ApplicationIcons.SaveMolecule);
      }
   }
}