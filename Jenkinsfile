pipeline {
  agent {
    node {
      label 'Windows'
    }
  }
  parameters {
    choice(name: 'sequencia',
      choices: 'A - Executor\nZ - Tudo',
      description: 'Opção de execução:')
  }
  stages {
    stage('Compilar') {
      steps {
        bat 'build Compilar'
      }
    }

    stage('Testes') {
      steps {
        //bat 'build Testes'
        //nunit(testResultsPattern: 'TestResult.xml', failIfNoResults: true)
        print "Testes ignorados"
      }
    }
  }
}