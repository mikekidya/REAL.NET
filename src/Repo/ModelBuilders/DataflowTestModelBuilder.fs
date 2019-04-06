namespace Repo.Metametamodels

open Repo.DataLayer
open Repo.CoreSemanticLayer
open Repo.InfrastructureSemanticLayer

/// Initializes repository with test model conforming to Dataflow Metamodel, actual program that can be written by end-user.
type DataflowTestModelBuilder() =
    interface IModelBuilder with
        member this.Build(repo: IRepo): unit =
            let infrastructure = InfrastructureSemantic(repo)
            let metamodel = Repo.findModel repo "DataflowMetamodel"
            let infrastructureMetamodel = infrastructure.Metamodel.Model

            let metamodelAbstractNode = Model.findNode metamodel "AbstractNode"
            let metamodelBlock = Model.findNode metamodel "BlockNode"

            let link = Model.findAssociationWithSource metamodelAbstractNode "target"

            let model = repo.CreateModel("DataflowTestModel", metamodel)

            let block1 = infrastructure.Instantiate model metamodelBlock
            let block2 = infrastructure.Instantiate model metamodelBlock

            let (-->) (src: IElement) dst =
                let aLink = infrastructure.Instantiate model link :?> IAssociation
                aLink.Source <- Some src
                aLink.Target <- Some dst
                dst

            block1 --> block2 |> ignore

            ()
