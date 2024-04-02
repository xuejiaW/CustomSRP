#ifndef CUSTOM_LIGHT_INCLUDED
#define CUSTOM_LIGHT_INCLUDED

struct Light {
    float3 color;
    float3 direction;
};

Light GetDirectionLight() {
    Light light;
    light.color = 1.0f;
    light.direction = float3(0.0f, 1.0f, 0.0f);
    return light;
}

#endif
