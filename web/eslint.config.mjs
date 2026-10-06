import eslint from '@eslint/js';
import tseslint from 'typescript-eslint';

export default tseslint.config(
  { ignores: ['dist/**', 'node_modules/**', '**/.*/**', 'next-env.d.ts', 'components/ui/**', 'hooks/use-mobile.ts'] },
  eslint.configs.recommended,
  ...tseslint.configs.recommended,
);
