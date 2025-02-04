﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class RenameExpressionProfileDTO : ValidatableDTO
   {
      private string _moleculeName;
      private string _category;
      private string _species;
      private IReadOnlyList<string> _prohibitedNames;
      private string _originalName = string.Empty;

      public RenameExpressionProfileDTO()
      {
         Rules.Add(RenameExpressionProfileDTORules.UniqueName);
         Rules.AddRange(RenameExpressionProfileDTORules.NonEmptyFieldRules);
      }

      public string Type { get; set; }

      public string Name => Constants.ContainerName.ExpressionProfileName(MoleculeName, Species, Category);

      public string MoleculeName
      {
         get => _moleculeName;
         set
         {
            _moleculeName = value;
            OnPropertyChanged(() => Name);
            OnPropertyChanged(() => MoleculeName);
         }
      }

      public string Category
      {
         get => _category;
         set
         {
            _category = value;
            OnPropertyChanged(() => Name);
            OnPropertyChanged(() => Category);
         }
      }

      public string Species
      {
         get => _species;
         set
         {
            _species = value;
            OnPropertyChanged(() => Name);
            OnPropertyChanged(() => Species);
         }
      }

      private static class RenameExpressionProfileDTORules
      {
         public static IEnumerable<IBusinessRule> NonEmptyFieldRules { get; } = new List<IBusinessRule>
         {
            createNonEmptyRule(item => item.Species, AppConstants.Validation.SpeciesCannotBeEmpty),
            createNonEmptyRule(item => item.Category, AppConstants.Validation.CategoryCannotBeEmpty),
            createNonEmptyRule(item => item.MoleculeName, AppConstants.Validation.MoleculeNameCannotBeEmpty)
         };

         private static IBusinessRule createNonEmptyRule(Expression<Func<RenameExpressionProfileDTO, string>> propertyToCheck, string errorCaption)
         {
            return CreateRule.For<RenameExpressionProfileDTO>()
               .Property(propertyToCheck)
               .WithRule((dto, proposedElement) => !string.IsNullOrEmpty(proposedElement))
               .WithError((dto, proposedElement) => errorCaption);
         }

         public static IBusinessRule UniqueName { get; } = CreateRule.For<RenameExpressionProfileDTO>()
            .Property(item => item.Name)
            .WithRule((dto, newName) => dto.isNameUnique(newName))
            .WithError((dto, newName) => Error.NameAlreadyExists(newName));
      }

      private bool isNameUnique(string newName)
      {
         if (_prohibitedNames == null)
            return true;

         if (allowCaseOnlyRename && differByCaseOnly(newName, _originalName))
            return true;

         return !_prohibitedNames.Contains(newName.ToLower());
      }

      /// <summary>
      ///    Checks that <paramref name="newName" /> differs from <paramref name="originalName" /> only in case.
      ///    That means that if the string is identical, this check will return false.
      /// </summary>
      private bool differByCaseOnly(string newName, string originalName)
      {
         return string.Equals(newName, originalName, StringComparison.OrdinalIgnoreCase) && !string.Equals(newName, originalName, StringComparison.Ordinal);
      }

      private bool allowCaseOnlyRename => !string.IsNullOrEmpty(_originalName);

      public void AddForbiddenNames(IReadOnlyList<string> prohibitedNames)
      {
         _prohibitedNames = prohibitedNames.Select(x => x.ToLower()).ToList();
      }

      public void AllowCaseOnlyChangesFor(string originalName)
      {
         _originalName = originalName;
      }
   }
}