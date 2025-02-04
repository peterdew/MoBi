﻿using MoBi.Presentation.DTO;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.Extensions
{
   public static class BreadCrumbsExtensions
   {
      public static bool HasAtLeastOneValue<T>(this IEnumerable<BreadCrumbsDTO<T>> breadcrumbs, int pathElementIndex) where T : IValidatable, INotifier
      {
         return breadcrumbs.Select(x => x.PathElementByIndex(pathElementIndex)).Any(x => !string.IsNullOrEmpty(x));
      }
   }
}
