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
          ls -al
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

            echo "Publish folder contents:"
            ls -al ./publish
          '''
        }
      }
    }
    
   stage('Archive Artifacts') {
      steps {
        // grab the published output from the workspace so Jenkins keeps it
        archiveArtifacts artifacts: 'YumBlazor/publish/**', fingerprint: true
      }
    }
  }
}