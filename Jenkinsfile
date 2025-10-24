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

        stage('Build') {
            steps {
                dir('YumBlazor') {
                    sh '''
                    dotnet restore
                    dotnet build --configuration Release /clp:ErrorsOnly
                    dotnet publish --configuration Release --output ./publish
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
