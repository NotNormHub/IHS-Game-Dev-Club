shader_type canvas_item;

void fragment() {
	vec4 pixelColor = texture(TEXTURE, UV);
	COLOR = vec4(pixelColor.r * (1.0 - (abs(UV.x - .5) * 2.0)), 0, pixelColor.b * abs(UV.x - .5), pixelColor.a);
}
