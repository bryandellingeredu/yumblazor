pipeline {
    agent any

    stages {
        stage('Diagnose') {
            steps {
                sh '''
                whoami
                id
                which dotnet
                dotnet --info
                '''
            }
        }

        stage('Restore') {
            steps {
                sh '''
                dotnet restore YumBlazor/YumBlazor.csproj
                dotnet restore YumBlazor.Tests/YumBlazor.Tests.csproj
                '''
            }
        }

       stage('Unit Tests') {
            steps {
                sh '''
                dotnet test --configuration Release \
                            --logger "junit;LogFileName=test-results.xml" \
                            /clp:ErrorsOnly
                '''
            }
            post {
                always {
                    junit 'YumBlazor.Tests/test-results.xml'
                }
            }
        }

        stage('Publish') {
            steps {
                dir('YumBlazor') {
                    sh '''
                    dotnet publish --configuration Release --output ./publish /clp:ErrorsOnly
                    '''
                }
            }
        }

        stage('Archive Published App') {
            steps {
                dir('YumBlazor') {
                    archiveArtifacts artifacts: 'publish/**'
                }
            }
        }
    }
}
