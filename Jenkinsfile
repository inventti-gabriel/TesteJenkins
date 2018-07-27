pipeline {
  agent any
  parameters {
    choice(name: 'sequencia',
      choices: 'A - Executor\nZ - Tudo',
      description: 'Opção de execução:')
  }
  stages {
    stage('Executor') {
      steps {
        bat 'build Executor'
      }
    }
  }
}