tag=$1
if [ -z "$tag" ]; then
  echo "Usage: $0 <tag>"
  exit 1
fi
region=us-east-1
aws_account_id=459427504023
docker_image_name=gamma-judge-api
aws ecr get-login-password --region $region | docker login --username AWS --password-stdin $aws_account_id.dkr.ecr.$region.amazonaws.com
docker tag $docker_image_name $aws_account_id.dkr.ecr.$region.amazonaws.com/$docker_image_name:$tag
docker push $aws_account_id.dkr.ecr.$region.amazonaws.com/$docker_image_name:$tag