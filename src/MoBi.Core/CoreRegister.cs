using System.Collections.Generic;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Comparison;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Domain.Services;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Helper;
using MoBi.Core.Reporting;
using MoBi.Core.Serialization.Converter;
using MoBi.Core.Services;
using OSPSuite.Core;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.FuncParser;
using OSPSuite.Infrastructure.Export;
using OSPSuite.Infrastructure.Import;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.Infrastructure.Serialization;
using OSPSuite.Infrastructure.Serialization.ORM.History;
using OSPSuite.TeXReporting;
using OSPSuite.Utility.Container;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Core
{
   public class CoreRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<CoreRegister>();
            scan.ExcludeType<MoBiDimensionFactory>();
            scan.ExcludeType<MoBiContext>();

            scan.ExcludeType<MoBiDimensionFactory>();
            scan.ExcludeType<MoBiConfiguration>();
            scan.ExcludeType<ObjectTypeResolver>();
            scan.ExcludeType<FormulaTypeCaptionRepository>();
            scan.ExcludeType<GroupRepository>();
            scan.ExcludeType<ClipboardManager>();
            scan.ExcludeType<ApplicationSettings>();
            scan.ExcludeType<MoBiLogger>();
            scan.ExcludeNamespaceContainingType<IMoBiObjectConverter>();
            scan.ExcludeNamespaceContainingType<ProjectReporter>();
            scan.ExcludeNamespaceContainingType<MoBiSimulationDiffBuilder>();
            scan.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
         });

         container.Register<IMoBiContext, IOSPSuiteExecutionContext, IWorkspace, MoBiContext>(LifeStyle.Singleton);
         container.Register<OSPSuite.Core.IApplicationSettings, IApplicationSettings, ApplicationSettings>(LifeStyle.Singleton);
         container.Register<IMoBiDimensionFactory, IDimensionFactory, MoBiDimensionFactory>(LifeStyle.Singleton);
         container.Register<IObjectTypeResolver, ObjectTypeResolver>(LifeStyle.Singleton);
         container.Register<FormulaTypeCaptionRepository, FormulaTypeCaptionRepository>(LifeStyle.Singleton);
         container.Register<IGroupRepository, GroupRepository>(LifeStyle.Singleton);
         container.Register<IClipboardManager, ClipboardManager>(LifeStyle.Singleton);
         container.Register<ICloneManager, CloneManagerForBuildingBlock>(LifeStyle.Singleton);

         container.Register<IProjectRetriever, MoBiProjectRetriever>();
         container.Register<IHistoryManager, MoBiHistoryManager>();
         container.Register<IObjectIdResetter, ObjectIdResetter>();
         container.Register<ISetParameterTask, ParameterTask>();
         container.Register<ITransferOptimizedParametersToSimulationsTask, TransferOptimizedParametersToSimulationsTask<IMoBiContext>>();

         //Register application specific core objects
         container.Register<IObjectBaseFactory, ObjectBaseFactory>(LifeStyle.Singleton);
         container.Register<IFullPathDisplayResolver, FullPathDisplayResolver>(LifeStyle.Singleton);

         //Register opened types generics
         container.Register(typeof(IEntitiesInBuildingBlockRetriever<>), typeof(EntitiesInBuildingBlockRetriever<>));
         container.Register<IList<IDimensionMergingInformation>, List<IDimensionMergingInformation>>(LifeStyle.Singleton);

         //Register abstract factories
         container.RegisterFactory<IHistoryManagerFactory>();
         container.RegisterFactory<IDiagramManagerFactory>();

         container.Register<DimensionParser, DimensionParser>();

         registerSerializers(container);

         registerImporter(container);

         registerReporters(container);

         registerComparers(container);

         registerConverters(container);
      }

      private void registerSerializers(IContainer container)
      {
         container.AddRegister(x => x.FromType<InfrastructureSerializationRegister>());
      }

      private void registerImporter(IContainer container)
      {
         container.AddRegister(x => x.FromType<InfrastructureImportRegister>());
      }

      private static void registerReporters(IContainer container)
      {
         container.AddRegister(x => x.FromType<ReportingRegister>());
         container.AddRegister(x => x.FromType<InfrastructureReportingRegister>());
         container.AddRegister(x => x.FromType<InfrastructureExportRegister>());

         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<CoreRegister>();
            scan.IncludeNamespaceContainingType<ProjectReporter>();
            scan.WithConvention<ReporterRegistrationConvention>();
         });
      }

      private static void registerComparers(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<CoreRegister>();
            scan.IncludeNamespaceContainingType<MoBiSimulationDiffBuilder>();
            scan.WithConvention<RegisterTypeConvention<IDiffBuilder>>();
         });
      }

      private static void registerConverters(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<CoreRegister>();
            scan.IncludeNamespaceContainingType<IMoBiObjectConverter>();

            scan.ExcludeType<MoBiObjectConverterFinder>();
            scan.ExcludeType<ProjectConverterLogger>();
            scan.WithConvention<RegisterTypeConvention<IMoBiObjectConverter>>();
         });

         //Needs to be registered as singleton
         container.Register<IMoBiObjectConverterFinder, MoBiObjectConverterFinder>(LifeStyle.Singleton);
         container.Register<IProjectConverterLogger, ProjectConverterLogger>(LifeStyle.Singleton);
      }
   }
}