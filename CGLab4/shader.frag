#version 330 core

const int DarkBlue = 0;
const int Green = 1;

out vec4 color;

uniform int colorMode;

void main()
{
	if (colorMode == DarkBlue)
		color = vec4(0.0f, 0.2f, 0.4f, 1.0f);
	if (colorMode == Green)
		color = vec4(0.5f, 1.0f, 0.5f, 1.0f);
}