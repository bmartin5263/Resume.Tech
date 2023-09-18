#!/bin/bash -e

# NEW_ECR_IMAGE=$(aws ecr describe-images --output json --repository-name resumeTech --query 'sort_by(imageDetails,& imagePushedAt)[-1].imageTags[0]' | jq . --raw-output)
NEW_ECR_IMAGE="112075753706.dkr.ecr.us-east-1.amazonaws.com/resumeTech:$(<version/Version)"
TASK_DEFINITION=$(aws ecs describe-task-definition --region "us-east-1" --task-definition "ResumeTech")
NEW_TASK_DEFINITION=$(echo $TASK_DEFINITION | jq --arg IMAGE "$NEW_ECR_IMAGE" '.taskDefinition | .containerDefinitions[0].image = $IMAGE | del(.taskDefinitionArn) | del(.revision) | del(.status) | del(.requiresAttributes) | del(.compatibilities) |  del(.registeredAt)  | del(.registeredBy)')
NEW_TASK_INFO=$(aws ecs register-task-definition --region "us-east-1" --cli-input-json "$NEW_TASK_DEFINITION")
NEW_REVISION=$(echo $NEW_TASK_INFO | jq '.taskDefinition.revision')
aws ecs update-service --region "us-east-1" --cluster "ResumeTechCluster" --service "ResumeTech" --task-definition "ResumeTech":${NEW_REVISION}