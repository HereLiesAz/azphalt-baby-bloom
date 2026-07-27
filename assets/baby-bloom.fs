/*{
  "DESCRIPTION": "A soft pink beauty bloom \u2014 brightened skin tones, warm rosy highlights, and gently reduced contrast for that clean pastel look.",
  "CATEGORIES": ["Guillotine", "Stylize"],
  "INPUTS": [
  {
    "NAME": "inputImage",
    "TYPE": "image"
  },
  {
    "NAME": "bloom",
    "TYPE": "float",
    "DEFAULT": 0.5,
    "MIN": 0.0,
    "MAX": 1.0
  },
  {
    "NAME": "pink",
    "TYPE": "float",
    "DEFAULT": 0.45,
    "MIN": 0.0,
    "MAX": 1.0
  }
]
}*/
void main() {
  vec2 uv = isf_FragNormCoord;
  vec2 t = 1.0 / RENDERSIZE;
  vec4 c = IMG_THIS_PIXEL(inputImage);

  vec3 soft = c.rgb;
  soft += IMG_NORM_PIXEL(inputImage, uv + vec2( t.x*3.0, 0.0)).rgb;
  soft += IMG_NORM_PIXEL(inputImage, uv + vec2(-t.x*3.0, 0.0)).rgb;
  soft += IMG_NORM_PIXEL(inputImage, uv + vec2(0.0,  t.y*3.0)).rgb;
  soft += IMG_NORM_PIXEL(inputImage, uv + vec2(0.0, -t.y*3.0)).rgb;
  soft /= 5.0;

  float luma = dot(soft, vec3(0.299, 0.587, 0.114));
  vec3 bloomed = c.rgb + soft * smoothstep(0.45, 1.0, luma) * bloom * 0.7;

  // Rosy highlights, cool-neutral shadows — a pastel split that flatters skin.
  vec3 rose = vec3(1.0, 0.82, 0.88);
  vec3 tinted = mix(bloomed, bloomed * rose, pink * smoothstep(0.25, 1.0, luma));

  // Gentle contrast reduction toward mid grey keeps it airy rather than punchy.
  vec3 airy = mix(tinted, tinted * 0.85 + vec3(0.12), 0.35);
  gl_FragColor = vec4(airy, c.a);
}
