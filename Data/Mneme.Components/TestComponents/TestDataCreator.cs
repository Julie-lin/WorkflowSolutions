using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AppData;
using Workflow.Data;

namespace Mneme.Components
{
    public class TestDataCreator
    {
        public static List<ComponentNode> CreateTreeTopInputComponentTestData()
        {
            List<ComponentNode> ret = new List<ComponentNode>();

            Guid startupId = Guid.NewGuid();
            var startupParam = new StartupComponentNode()
            {
                Id = startupId,
                //BatchInitializeExcutionName = "BatchInitializer",
               // CompopnentExcutionName = "ClientStartupComponent",
                ComponentName = "Client  Startup Component",
                StartNode = true,
                TreeExecutionTag = "execution tag abc",
            };
            ret.Add(startupParam);

            Guid treeTopId = Guid.NewGuid();
            TreeTopNode compParam = new TreeTopNode()
            {
                Id = treeTopId,
                CompopnentExcutionName = "TreeTopExecutable",
                ComponentName = "TreeTop Executable ",
                StartNode = false
            };
            compParam.ParentIdList.Add(startupId);
            ret.Add(compParam);
          

            //Guid groupId = Guid.NewGuid();
            //compParam = new ClientComponentNode()
            //{
            //    Id = ClientExcutableComponentBBCId,
            //    CompopnentExcutionName = "ClientGroupExecutable",
            //    ComponentName = "group executable",
            //    StartNode = false,
            //    CompNodeValidation = NodeValidationType.Group
            //};
            //compParam.ParentIdList.Add(ClientExcutableComponentBBCId);

            //ret.Add(compParam);
            return ret;

        }
        public static List<ComponentNode> CreateTreeTopHeloWoldTestData()
        {
            List<ComponentNode> ret = new List<ComponentNode>();

            Guid startupId = Guid.NewGuid();
            var startupParam = new StartupComponentNode()
            {
                Id = startupId,
                //BatchInitializeExcutionName = "BatchInitializer",
                // CompopnentExcutionName = "ClientStartupProcess",
                ComponentName = "Client  Startup Component",
                StartNode = true,
                TreeExecutionTag = "execution tag abc",
            };
            ret.Add(startupParam);

            Guid treeTopId = Guid.NewGuid();
            TreeTopNode treeTopNode = new TreeTopNode()
            {
                Id = treeTopId,
                CompopnentExcutionName = "TreeTopExecutable",
                ComponentName = "TreeTop Executable ",
                StartNode = false
            };
            treeTopNode.ParentIdList.Add(startupId);
            ret.Add(treeTopNode);

            Guid helloWorldId = Guid.NewGuid();
            HelloWorldNode helloWorldNode = new HelloWorldNode()
            {
                Id = helloWorldId,
                CompopnentExcutionName = "HelloWorldProcess",
                ComponentName = "Hello World ",
                StartNode = false
            };
            helloWorldNode.ParentIdList.Add(treeTopId);
            ret.Add(helloWorldNode);


            //Guid groupId = Guid.NewGuid();
            //compParam = new ClientComponentNode()
            //{
            //    Id = ClientExcutableComponentBBCId,
            //    CompopnentExcutionName = "ClientGroupExecutable",
            //    ComponentName = "group executable",
            //    StartNode = false,
            //    CompNodeValidation = NodeValidationType.Group
            //};
            //compParam.ParentIdList.Add(ClientExcutableComponentBBCId);

            //ret.Add(compParam);
            return ret;

        }
        public static List<ComponentNode> CreateSimplePeakDetectTestData()
        {
            List<ComponentNode> ret = new List<ComponentNode>();

            Guid startupId = Guid.NewGuid();
            var startupParam = new StartupComponentNode()
            {
                Id = startupId,
                //BatchInitializeExcutionName = "BatchInitializer",
                // CompopnentExcutionName = "ClientStartupProcess",
                ComponentName = "Client  Startup Component",
                StartNode = true,
                TreeExecutionTag = "execution tag abc",
            };
            ret.Add(startupParam);

            Guid helloWorldId = Guid.NewGuid();
            HelloWorldNode helloWorldNode = new HelloWorldNode()
            {
                Id = helloWorldId,
                CompopnentExcutionName = "HelloWorldProcess",
                ComponentName = "Hello World ",
                StartNode = false
            };
            helloWorldNode.ParentIdList.Add(startupId);
            ret.Add(helloWorldNode);
            return ret;
        }
        public static List<ComponentNode> CreateMutipleInputComponentTestData()
        {
            List<ComponentNode> ret = new List<ComponentNode>();

            Guid startupId = Guid.NewGuid();
            var startupParam = new StartupComponentNode()
            {
                Id = startupId,
                BatchInitializeExcutionName = "BatchInitializer",
                CompopnentExcutionName = "ClientStartupProcess",
                ComponentName = "Client  Startup Component",
                StartNode = true,
                TreeExecutionTag = "execution tag abc",
            };
            ret.Add(startupParam);

            //first layer A, B C D
            Guid ClientExcutableComponentAId = Guid.NewGuid();
            ClientComponentNode compParam = new ClientComponentNode()
            {
                Id = ClientExcutableComponentAId,
                CompopnentExcutionName = "ClientExcutableComponentA",
                ComponentName = "Client Excutable Component A",
                StartNode = false
            };
            compParam.ParentIdList.Add(startupId);
            ret.Add(compParam);

            Guid ClientExcutableComponentBId = Guid.NewGuid();
            compParam = new ClientComponentNode()
            {
                Id = ClientExcutableComponentBId,
                CompopnentExcutionName = "ClientExcutableComponentB",
                ComponentName = "Client Excutable Component B",
                StartNode = false

            };
            compParam.ParentIdList.Add(startupId);
            ret.Add(compParam);

            Guid ClientExcutableComponentCId = Guid.NewGuid();
            compParam = new ClientComponentNode()
            {
                Id = ClientExcutableComponentCId,
                CompopnentExcutionName = "ClientExcutableComponentC",
                ComponentName = "Client Excutable Component C",
                StartNode = false
            };
            compParam.ParentIdList.Add(startupId);
            ret.Add(compParam);

            Guid ClientExcutableComponentDId = Guid.NewGuid();
            compParam = new ClientComponentNode()
            {
                Id = ClientExcutableComponentDId,
                CompopnentExcutionName = "ClientExcutableComponentD",
                ComponentName = "Client Excutable Component D",
                StartNode = false
            };
            compParam.ParentIdList.Add(startupId);
            ret.Add(compParam);


            //second layer AA, AB(parent:A, B), BB, CC DD
            Guid ClientExcutableComponentAAId = Guid.NewGuid();
            compParam = new ClientComponentNode()
            {
                Id = ClientExcutableComponentAAId,
                CompopnentExcutionName = "ClientExcutableComponentAA",
                ComponentName = "ClientExcutableComponentAA",
                StartNode = false

            };
            compParam.ParentIdList.Add(ClientExcutableComponentAId);
            ret.Add(compParam);

            Guid ClientExcutableComponentABId = Guid.NewGuid();
            compParam = new ClientComponentNode()
            {
                Id = ClientExcutableComponentABId,
                CompopnentExcutionName = "ClientExcutableComponentAB",
                ComponentName = "ClientExcutableComponentAB",
                StartNode = false
            };
            compParam.ParentIdList.Add(ClientExcutableComponentAId);
            compParam.ParentIdList.Add(ClientExcutableComponentBId);
            ret.Add(compParam);


            Guid ClientExcutableComponentBBId = Guid.NewGuid();
            compParam = new ClientComponentNode()
            {
                Id = ClientExcutableComponentBBId,
                CompopnentExcutionName = "ClientExcutableComponentBB",
                ComponentName = "Client Excutable ComponentBB",
                StartNode = false
            };
            compParam.ParentIdList.Add(ClientExcutableComponentBId);
            ret.Add(compParam);


            //third layer AA1 ABBB (parent: AB, CC, DD) 
            Guid ClientExcutableComponentAA1Id = Guid.NewGuid();
            compParam = new ClientComponentNode()
            {
                Id = ClientExcutableComponentAA1Id,
                CompopnentExcutionName = "ClientExcutableComponentAA1",
                ComponentName = "Client Excutable ComponentAA1",
                StartNode = false
            };
            compParam.ParentIdList.Add(ClientExcutableComponentAAId);
            ret.Add(compParam);

            Guid ClientExcutableComponentABBBId = Guid.NewGuid();
            compParam = new ClientComponentNode()
            {
                Id = ClientExcutableComponentABBBId,
                CompopnentExcutionName = "ClientExcutableComponentABBB",
                ComponentName = "Client Excutable ComponentABBB",
                StartNode = false
            };
            compParam.ParentIdList.Add(ClientExcutableComponentABId);
            compParam.ParentIdList.Add(ClientExcutableComponentBBId);
            ret.Add(compParam);

            Guid ClientExcutableComponentBBCId = Guid.NewGuid();
            compParam = new ClientComponentNode()
            {
                Id = ClientExcutableComponentBBCId,
                CompopnentExcutionName = "ClientExcutableComponentBBC",
                ComponentName = "Client Excutable ComponentBBC",
                StartNode = false
            };
            compParam.ParentIdList.Add(ClientExcutableComponentBBId);
            compParam.ParentIdList.Add(ClientExcutableComponentCId);
            compParam.ParentIdList.Add(ClientExcutableComponentDId);
            ret.Add(compParam);


            ////group layer
            //Guid groupId = Guid.NewGuid();
            //compParam = new ClientComponentNode()
            //{
            //    Id = ClientExcutableComponentBBCId,
            //    CompopnentExcutionName = "ClientGroupExecutable",
            //    ComponentName = "group executable",
            //    StartNode = false,
            //    CompNodeValidation = NodeValidationType.Group
            //};
            //compParam.ParentIdList.Add(ClientExcutableComponentBBCId);

            //ret.Add(compParam);


            return ret;
        }
        public static List<string> CreateRawFileTestData()
        {
            List<string> rawfiles = new List<string>()
                                        {
                                            @"C:\Xcalibur\examples\data\drugx_01.raw",
                                            @"C:\Xcalibur\examples\data\drugx_02.raw",
                                            @"C:\Xcalibur\examples\data\drugx_03.raw"
                                        };
            return rawfiles;
        }
    }
}
