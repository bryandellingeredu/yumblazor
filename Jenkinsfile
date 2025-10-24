pipeline {
    agent any

      environment {
        DOCKER_HOST = 'tcp://host.docker.internal:2375'
        }

      stages {
            stage('Diagnose') {
                steps {
                    sh '''
                    echo "DOCKER_HOST=$DOCKER_HOST"
                    which docker || true
                    docker version
                    ls -R
                    '''
                }
            }
            stage('Build'){
                agent{
                    docker{
                        image 'mcr.microsoft.com/dotnet/sdk:9.0'
                        reuseNode true
                    }
                }
                 steps {
                    sh '''
                        cd Yumblazor
                        dotnet --info
                        dotnet restore
                        dotnet build --configuration Release
                        dotnet publish --configuration Release --output ./publish
                        '''
               }
            }
      }
}