namespace Repo.Metametamodels

open Repo.DataLayer
open Repo.CoreSemanticLayer
open Repo.InfrastructureSemanticLayer

/// Initializes repository with test model conforming to Robots Metamodel, actual program that can be written by end-user.
type DataflowTestModelBuilder() =
    interface IModelBuilder with
        member this.Build(repo: IRepo): unit =
            let infrastructure = InfrastructureSemantic(repo)
            let metamodel = Repo.findModel repo "DataflowMetamodel"
            let infrastructureMetamodel = infrastructure.Metamodel.Model

            let metamodelAbstractNode = Model.findNode metamodel "AbstractNode"
            //let metamodelInitialNode = Model.findNode metamodel "InitialNode"
            let metamodelFinalNode = Model.findNode metamodel "FinalNode"
            let metamodelBlock = Model.findNode metamodel "BlockNode"

            let link = Model.findAssociationWithSource metamodelAbstractNode "target"

            let model = repo.CreateModel("DataflowTestModel", metamodel)

            //let initialNode = infrastructure.Instantiate model metamodelInitialNode
            let finalNode = infrastructure.Instantiate model metamodelFinalNode

            let block = infrastructure.Instantiate model metamodelBlock

            let (-->) (src: IElement) dst =
                let aLink = infrastructure.Instantiate model link :?> IAssociation
                aLink.Source <- Some src
                aLink.Target <- Some dst
                dst

            //initialNode --> 
            block --> finalNode |> ignore

            ()
