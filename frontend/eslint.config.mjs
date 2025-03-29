import withNuxt from './.nuxt/eslint.config.mjs';
import jsdoc from 'eslint-plugin-jsdoc';

export default withNuxt({
  plugins: { jsdoc },
  rules: {
    'vue/html-self-closing': [
      'warn',
      {
        html: {
          void: 'any',
          normal: 'any',
          component: 'always',
        },
      },
    ],
  },
})
  .prepend(jsdoc.configs['flat/recommended'])
  .overrideRules({
    'jsdoc/require-param-type': 'off',
    'jsdoc/require-returns-type': 'off',
  });
