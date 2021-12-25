#version 330 core

const int Red = 0;
const int Green = 1;
const int Cyan = 2;

out vec4 color;

uniform int ColorMode;

void main()
{
	if (ColorMode == Red)
		color = vec4(0.7f, 0.0f, 0.0f, 1.0f);
	if (ColorMode == Green)
		color = vec4(0.5f, 1.0f, 0.5f, 1.0f);
    if (ColorMode == Cyan)
        color = vec4(0.0f, 1.0f, 1.0f, 1.0f);
}