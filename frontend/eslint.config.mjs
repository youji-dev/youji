import withNuxt from './.nuxt/eslint.config.mjs';
import jsdoc from 'eslint-plugin-jsdoc';

export default withNuxt({
  plugins: { jsdoc },
  rules: {
    '@typescript-eslint/no-explicit-any': 'off',
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
    'vue/no-mutating-props': [
      'error',
      {
        shallowOnly: true,
      },
    ],
  },
})
  .prepend(jsdoc.configs['flat/recommended'])
  .overrideRules({
    'jsdoc/require-param-type': 'off',
    'jsdoc/require-returns-type': 'off',
  });
