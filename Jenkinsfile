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
                    '''
                }
            }
      }
}