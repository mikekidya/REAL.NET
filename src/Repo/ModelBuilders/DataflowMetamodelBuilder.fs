namespace Repo.Metametamodels

open Repo
open Repo.DataLayer
open Repo.InfrastructureSemanticLayer

/// Initializes repository with Robots Metamodel, first testing metamodel of a real language.
type DataflowMetamodelBuilder() =
    interface IModelBuilder with
        member this.Build(repo: IRepo): unit =
            let infrastructure = InfrastructureSemanticLayer.InfrastructureSemantic(repo)
            let metamodel = infrastructure.Metamodel.Model

            let find name = CoreSemanticLayer.Model.findNode metamodel name
            let findAssociation node name = CoreSemanticLayer.Model.findAssociationWithSource node name

            let metamodelElement = find "Element"
            let metamodelNode = find "Node"
            let metamodelGeneralization = find "Generalization"
            let metamodelAssociation = find "Association"
            let metamodelAttribute = find "Attribute"

            let metamodelStringNode = find "String"
            let metamodelBooleanNode = find "Boolean"
            let metamodelMetatypeNode = find "Metatype"
            let metamodelAttributeKindNode = find "AttributeKind"

            let attributesAssociation = findAssociation metamodelElement "attributes"

            let shapeAssociation = findAssociation metamodelElement "shape"
            let isAbstractAssociation = findAssociation metamodelElement "isAbstract"
            let instanceMetatypeAssociation = findAssociation metamodelElement "instanceMetatype"

            let attributeKindAssociation = findAssociation metamodelAttribute "kind"
            let attributeStringValueAssociation = findAssociation metamodelAttribute "stringValue"

            let edgeTargetNameAssociation = findAssociation metamodelAssociation "targetName"

            let model = repo.CreateModel("DataflowMetamodel", metamodel)

            let (~+) (name, shape, isAbstract) =
                let node = infrastructure.Instantiate model metamodelNode :?> INode
                node.Name <- name
                infrastructure.Element.SetAttributeValue node "shape" shape
                infrastructure.Element.SetAttributeValue node "isAbstract" (if isAbstract then "true" else "false")
                infrastructure.Element.SetAttributeValue node "instanceMetatype" "Metatype.Node"

                node

            let (--|>) (source: IElement) target =
                model.CreateGeneralization(metamodelGeneralization, source, target) |> ignore

            let (--->) (source: IElement) (target, targetName, linkName) =
                let edge = infrastructure.Instantiate model metamodelAssociation :?> IAssociation
                edge.Source <- Some source
                edge.Target <- Some target
                edge.TargetName <- targetName

                infrastructure.Element.SetAttributeValue edge "shape" "View/Pictures/edge.png"
                infrastructure.Element.SetAttributeValue edge "isAbstract" "false"
                infrastructure.Element.SetAttributeValue edge "instanceMetatype" "Metatype.Edge"
                infrastructure.Element.SetAttributeValue edge "name" linkName

                edge

            let abstractNode = +("AbstractNode", "", true)
            //let initialNode = +("InitialNode", "View/Pictures/initialBlock.png", false)
            //let finalNode = +("FinalNode", "View/Pictures/finalBlock.png", false)

            let blockNode = +("BlockNode", "View/Pictures/rectangle.png", false);
            infrastructure.Element.AddAttribute blockNode "name" "AttributeKind.String" "unnamed"
            infrastructure.Element.AddAttribute blockNode "output type" "AttributeKind.String" ""

            let link = abstractNode ---> (abstractNode, "target", "Link")

            //finalNode --|> abstractNode
            blockNode --|> abstractNode
       
            ()

