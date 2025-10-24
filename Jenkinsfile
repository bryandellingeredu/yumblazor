pipeline {
    agent any

    stages {
        stage('Diagnose') {
            steps {
                sh '''
                echo "=== whoami / groups ==="
                whoami
                id

                echo "=== dotnet info ==="
                which dotnet
                dotnet --info

                echo "=== repo contents ==="
                ls -R
                '''
            }
        }

        stage('Build & Publish') {
            steps {
                dir('YumBlazor') {
                    sh '''
                    dotnet restore
                    dotnet build --configuration Release /clp:ErrorsOnly
                    dotnet publish --configuration Release --output ./publish /clp:ErrorsOnly
                    '''
                }
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
                    // Store the TRX so you have proof of results for that build
                    archiveArtifacts artifacts: 'YumBlazor.Tests/TestResults/test-results.trx'
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
