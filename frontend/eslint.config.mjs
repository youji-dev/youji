import withNuxt from './.nuxt/eslint.config.mjs';
import jsdoc from 'eslint-plugin-jsdoc';

export default withNuxt({
  plugins: { jsdoc },
}).prepend(jsdoc.configs['flat/recommended']);
