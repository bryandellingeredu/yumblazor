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
        dir('YumBlazor.Tests') {
            sh '''
            dotnet test --configuration Release \
                        --logger "trx;LogFileName=test-results.trx" \
                        /clp:ErrorsOnly

            echo "=== After test run, here's TestResults dir ==="
            ls -R
            '''
        }
    }
    post {
        always {
            junit 'YumBlazor.Tests/**/test-results.trx'
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
