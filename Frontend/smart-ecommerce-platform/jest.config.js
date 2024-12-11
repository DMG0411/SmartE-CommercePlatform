module.exports = {
  preset: 'jest-preset-angular',
  setupFilesAfterEnv: ['<rootDir>/src/setup.jest.ts'],
  moduleNameMapper: {
    '@app/(.*)': '<rootDir>/src/app/$1',
    'environments/(.*)': '<rootDir>/src/environments/$1', 
  },
  coverageDirectory: '<rootDir>/jest-coverage',
  collectCoverage: true,
  collectCoverageFrom: [
    'src/app/**/*.ts',
    '!src/app/**/*.module.ts',
    '!src/app/**/*-spec.ts',
  ],
  coverageReporters: ['html', 'text-summary'], 
}