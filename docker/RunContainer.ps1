$projectDirectory = Join-Path -Path $pwd -ChildPath 'docker'
docker-compose --project-directory $projectDirectory up