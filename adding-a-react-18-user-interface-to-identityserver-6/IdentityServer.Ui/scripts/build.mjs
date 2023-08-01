import { build } from "esbuild";
import { cp } from "fs/promises";

await build({
  sourcemap: true,
  minify: true,
  define: {
    "process.env.NODE_ENV": "'production'",
  },
  bundle: true,
  outfile: "./build/index.js",
  entryPoints: ["./src/index.tsx"],
});

await cp("./public", "./build", { recursive: true });
