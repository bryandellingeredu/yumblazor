pipeline {
    agent any



      stages {
            stage('Diagnose') {
                steps {
                    sh '''
                   echo "=== whoami / groups ==="
                    whoami
                    id

                    echo "=== docker cli ==="
                    which docker || echo "docker CLI not found"
                    docker version || echo "cannot talk to docker daemon"

                    echo "=== repo contents ==="
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