pipeline {
  agent {
    kubernetes {
              yaml """
        apiVersion: v1
        kind: Pod
        spec:
          containers:
          - name: dotnet-builder
            image: mcr.microsoft.com/dotnet/sdk:9.0
            command: ['cat']
            tty: true
        """
      defaultContainer 'dotnet-builder'
    }
  }

  stages {
    stage('Check env') {
      steps {
        sh '''
         echo "Node hostname: $(hostname)"
          dotnet --info
          pwd
        '''
      }
    }
    stage('Build & Publish') {
      steps {
        dir('YumBlazor') {
          sh '''
            echo "Restoring..."
            dotnet restore

            echo "Building Release..."
            dotnet build --configuration Release /clp:ErrorsOnly

            echo "Publishing Release..."
            dotnet publish --configuration Release --output ./publish /clp:ErrorsOnly
          '''
        }
      }
    }

     stage('Unit Tests') {
      steps {
        dir('YumBlazor.Tests') {
          sh '''
            echo "Running unit tests..."
            dotnet test --configuration Release \
                        --logger "trx;LogFileName=test-results.trx" \
                        /clp:ErrorsOnly
          '''
        }
      }
      post {
        always {
          // Archive the TRX results file so Jenkins keeps them even if tests fail
          archiveArtifacts artifacts: 'YumBlazor.Tests/TestResults/test-results.trx', excludes: '**/*.pdb'
        }
      }
    }
    
   stage('Archive Artifacts') {
      steps {
        // grab the published output from the workspace so Jenkins keeps it
          archiveArtifacts artifacts: 'YumBlazor/publish/**', excludes: '**/*.pdb'
      }
    }
  }
}