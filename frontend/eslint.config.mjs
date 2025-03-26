import withNuxt from './.nuxt/eslint.config.mjs';
import jsdoc from 'eslint-plugin-jsdoc';

export default withNuxt({
  plugins: { jsdoc },
  rules: {},
})
  .prepend(jsdoc.configs['flat/recommended'])
  .overrideRules({
    'jsdoc/require-param-type': 'off',
    'jsdoc/require-returns-type': 'off',
  });
