#version 330 core

const int DarkBlue = 0;
const int Green = 1;
const int Cyan = 2;
const int White = 3;
const int None = 4;

//struct Material {
//	vec3 Ka;
//	vec3 Kd;
//	vec3 Ks;
//	float p;
//};
//
//struct Light {
//	vec3 intensity;
//	vec3 position;
//	float attenuation;
//};
//
//uniform Material material;
//uniform Light light;
//uniform vec3 ambientIntensity;

//in vec3 ourColor;
out vec4 color;

uniform int ColorMode;

void main()
{
	if (ColorMode == DarkBlue)
		color = vec4(0.0f, 0.2f, 0.4f, 1.0f);
	if (ColorMode == Green)
		color = vec4(0.5f, 1.0f, 0.5f, 1.0f);
    if (ColorMode == Cyan)
        color = vec4(0.0f, 1.0f, 1.0f, 1.0f);
	if (ColorMode == White)
		color = vec4(1.0f, 1.0f, 1.0f, 1.0f);
	else {
		
	}
}