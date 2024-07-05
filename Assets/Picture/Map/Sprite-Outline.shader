// ����һ��Unity Shader���룬����Ϊ�����������Ч����

// Shader�����ƺ�����
Shader "Sprites/Outline" {
    // ���Զ���飬���ﶨ����Shader��Ҫ������
    Properties {
        // _MainTex�Ǿ������������Ϊ2D
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        // _Color�Ǿ������ɫ��ʹ��RGBA��ɫֵ
        _Color ("Tint", Color) = (1,1,1,1)
        // PixelSnap�������ض��룬��һ������
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0

        // ����Ч���Ŀ��غ���ɫ
        [PerRendererData] _Outline ("Outline", Float) = 0
        [PerRendererData] _OutlineColor("Outline Color", Color) = (1,1,1,1)

        //�������Լ�����Ĳ���ƫ�������ԣ����ڵ�������Ч����������(������C#�ű��ⲿ���Ʋ����Ĳ���ƫ����)
        [PerRendererData]  _SampleOffset("Sample Offset",Float) = 1
    }

    // SubShader������һ����Ⱦ���ú�Pass
    SubShader {
        // Tags��������Ⱦ���С��Ƿ����ͶӰ��
        Tags {
            "Queue"="Transparent" // ��Ⱦ��������Ϊ͸��
            "IgnoreProjector"="True" // ����ͶӰ
            "RenderType"="Transparent" // ��Ⱦ��������Ϊ͸��
            "PreviewType"="Plane" // Ԥ������
            "CanUseSpriteAtlas"="True" // ����ʹ�þ���ͼ��
        }

        // ��Ⱦ״̬����
        Cull Off // ���޳���
        Lighting Off // �رչ���Ч��
        ZWrite Off // �ر����д��
        Blend One OneMinusSrcAlpha // ���û��ģʽ������͸������

        // Pass������һ����Ⱦͨ��
        // Properties�����Զ��壬��Ҫ��Pass���ٴ������������޷���Pass��ʹ��
        Pass {
            // CGPROGRAM��ENDCG������HLSL����
            CGPROGRAM
            // ���嶥���Ƭ����ɫ������ڵ�
            #pragma vertex vert
            #pragma fragment frag
            // �����ָ��������ض���
            #pragma multi_compile _ PIXELSNAP_ON
            // ���Կ��أ�����ETC1��ʽ���ⲿAlphaͨ��
            #pragma shader_feature ETC1_EXTERNAL_ALPHA
            // ����Unity��CG��
            #include "UnityCG.cginc"

            // ���嶥�����ݽṹ
            struct appdata_t {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            // ����Ӷ�����ɫ����Ƭ����ɫ���Ľṹ
            struct v2f {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
            };

            // ����ȫ�ֱ���
            fixed4 _Color;
            float _Outline;
            fixed4 _OutlineColor;

            // ������ɫ��
            v2f vert(appdata_t IN) {
                v2f OUT;
                // ������λ��ת�����ü��ռ�
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                // Ӧ����ɫ����ɫ���
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                // ������������ض��룬��Ӧ��
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            // �������������
            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float4 _MainTex_TexelSize;
            // ����ƫ���������ڵ�������Ч����������
            float _SampleOffset;
            // ������������ĺ���
            fixed4 SampleSpriteTexture (float2 uv) {
                fixed4 color = tex2D (_MainTex, uv);

                #if ETC1_EXTERNAL_ALPHA
                // ���������ETC1_EXTERNAL_ALPHA�����ⲿ�������Alphaֵ
                color.a = tex2D (_AlphaTex, uv).r;
                #endif //ETC1_EXTERNAL_ALPHA

                return color;
            }

            // Ƭ����ɫ��
            fixed4 frag(v2f IN) : SV_Target {
                fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
                
                // �������Ч�������ҵ�ǰ���ز�͸��
                if (_Outline > 0 && c.a != 0) {

                    // ����ƫ���������ڵ�������Ч����������
                    float2 offest =float2(_MainTex_TexelSize.x*_SampleOffset, _MainTex_TexelSize.y*_SampleOffset);

                    // ��ȡ��Χ�ĸ����ص���ɫ
                    fixed4 pixelUp = tex2D(_MainTex, IN.texcoord + fixed2(0, offest.y));
                    fixed4 pixelDown = tex2D(_MainTex, IN.texcoord - fixed2(0, offest.y));
                    fixed4 pixelRight = tex2D(_MainTex, IN.texcoord + fixed2(offest.x, 0));
                    fixed4 pixelLeft = tex2D(_MainTex, IN.texcoord - fixed2(offest.x, 0));

                    // �����Χ�ĸ�����������һ��������͸���ģ�����Ⱦ����
                    if (pixelUp.a * pixelDown.a * pixelRight.a * pixelLeft.a == 0) {
                        // ʹ��������ɫ
                        c.rgba = fixed4(1, 1, 1, 1) * _OutlineColor;
                    }
                }

                // Ӧ��Ԥ��Alpha��ʹ����ɫֵ����Alpha����ʱ��ʾ
                c.rgb *= c.a;

                return c;
            }
            ENDCG
        }
    }
}