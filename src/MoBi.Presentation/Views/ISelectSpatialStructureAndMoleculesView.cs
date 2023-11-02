﻿using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectSpatialStructureAndMoleculesView : IModalView<ISelectSpatialStructureAndMoleculesPresenter>
   {
      void Show(SelectSpatialStructureAndMoleculesDTO dto);
   }
}