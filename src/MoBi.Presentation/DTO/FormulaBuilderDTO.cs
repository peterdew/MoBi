using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.DTO
{
   public class FormulaBuilderDTO : ObjectBaseDTO
   {
      public static FormulaBuilderDTO NULL = new FormulaBuilderDTO {Description = AppConstants.NullFormulaDescription, Name = AppConstants.Captions.FormulaNotAvailable};

      private FormulaBuilderDTO()
      {
      }

      public FormulaBuilderDTO(IObjectBase objectBase) : base(objectBase)
      {
      }

      public IReadOnlyList<FormulaUsablePathDTO> ObjectPaths { get; set; }
      public virtual IDimension Dimension { get; set; }
      public virtual string FormulaType { get; set; }
      public virtual string FormulaString { get; set; }

      public override string ToString()
      {
         return $"{Name} ({FormulaString})";
      }
   }
}