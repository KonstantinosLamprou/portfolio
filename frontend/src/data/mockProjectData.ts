import type { ContentBlock } from "./mockBlogPostData";

type projectData = {
  id: string,
  type: 'blog' | 'project',
  slug: string,
  title: string;
  author: string;
  date: Date;
  description: string;
  content: ContentBlock[];
  //was wäre es denn für ein Type wenn ich zum Beispiel Code Schnipsel habe aus dem Projekt -> wäre dies ein Bild oder einfach nut Text in einer besonderen Textbox?
  imgSrc: string;
  likes?: number;
  views?: number;
}

//Wo greift die ImgSrc ein?
// Mock daten
//project data wird aus dem backend geliefert
//das bild? bzw svg sollte auch geliefert werden können

export const projectsData: projectData[] = [
    {
        id: '1',
        type: 'project',
        title: 'HTTP',
        description: `hallo`,
        imgSrc: '/http.svg',
        slug: 'http',
        author: 'John Doe',
        date: new Date('2023-10-01'),
        likes: 100,
        views: 1000,
        content: [
          {
          id: "block-1",
          type: "heading",
          data: {
            level: 2,
            text: "Was sind Generics überhaupt?"
          }
        },
        {
          id: "block-2",
          type: "paragraph",
          data: {
            text: "Stell dir Generics wie Platzhalter für Datentypen vor. Anstatt dich auf `string` oder `number` festzulegen, sagst du TypeScript: <i>'Der Typ wird später definiert'</i>."
          }
        },
        {
          id: "block-3",
          type: "image",
          data: {
            src: "/http.svg",
            alt: "Eine Box, die jeden Typ aufnehmen kann",
            caption: "" // Optional, falls du eine Bildunterschrift willst
          }
        },
        {
          id: "block-4",
          type: "heading",
          data: {
            level: 3,
            text: "Ein praktisches Code-Beispiel"
          }
        },
        {
          id: "block-5",
          type: "code",
          data: {
            language: "typescript", // Wichtig, falls du später Syntax-Highlighting (z.B. mit highlight.js oder Prism) nutzt
            code: "function identity<Type>(arg: Type): Type {\n  return arg;\n}\n\nlet output = identity<string>('Hallo Welt');"
          }
        },
        {
          id: "block-6",
          type: "paragraph",
          data: {
            text: "Es geht hier um vieles um Ideen und um zu lernen und genau das möchte ich damit erreichen! "
          }
        },
        ],
    },
    {
        id: '2',
        type: 'project',
        title: 'Load Balancer',
        description: `dasfd`,
        imgSrc: '/load-balancer.svg',
        slug: 'loadbalancer',
        author: 'John Doe',
        date: new Date('2023-10-01'),
        likes: 150,
        views: 1500,
        content: [
          {
            id: "block-1",
            type: "heading",
            data: {
              level: 2,
              text: "Was sind Generics überhaupt?"
              }
          },
          {
            id: "block-2",
            type: "paragraph",
            data: {
              text: "Stell dir Generics wie Platzhalter für Datentypen vor. Anstatt dich auf `string` oder `number` festzulegen, sagst du TypeScript: <i>'Der Typ wird später definiert'</i>."
            }
          },
          {
            id: "block-3",
            type: "image",
            data: {
              src: "/http.svg",
              alt: "Eine Box, die jeden Typ aufnehmen kann",
              caption: "" // Optional, falls du eine Bildunterschrift willst
            }
          },
          {
          id: "block-4",
          type: "heading",
          data: {
            level: 3,
            text: "Ein praktisches Code-Beispiel"
          }
        },
        {
            id: "block-5",
            type: "code",
            data: {
              language: "typescript", // Wichtig, falls du später Syntax-Highlighting (z.B. mit highlight.js oder Prism) nutzt
              code: "function identity<Type>(arg: Type): Type {\n  return arg;\n}\n\nlet output = identity<string>('Hallo Welt');"
            }
          },
          {
            id: "block-6",
            type: "paragraph",
            data: {
              text: "Es geht hier um vieles um Ideen und um zu lernen und genau das möchte ich damit erreichen! "
            }
          },
        ],
    },
    {
        id: '3',
        type: 'project',
        title: 'Portfolio',
        description: `dsaf`,
        imgSrc: '/portfolio.svg',
        slug: 'portfolio',
        author: 'John Doe',
        date: new Date('2023-10-01'),
        likes: 2,
        views: 20,
        content: [
          {
            id: "block-1",
            type: "heading",
            data: {
              level: 2,
              text: "Was sind Generics überhaupt?"
            }
          },
          {
            id: "block-2",
            type: "paragraph",
            data: {
              text: "Stell dir Generics wie Platzhalter für Datentypen vor. Anstatt dich auf `string` oder `number` festzulegen, sagst du TypeScript: <i>'Der Typ wird später definiert'</i>."
            }
          },
          {
            id: "block-3",
            type: "image",
            data: {
              src: "/http.svg",
              alt: "Eine Box, die jeden Typ aufnehmen kann",
              caption: "" // Optional, falls du eine Bildunterschrift willst
            }
          },
          {
            id: "block-4",
            type: "heading",
            data: {
              level: 3,
              text: "Ein praktisches Code-Beispiel"
            }
          },
          {
            id: "block-5",
            type: "code",
            data: {
              language: "typescript", // Wichtig, falls du später Syntax-Highlighting (z.B. mit highlight.js oder Prism) nutzt
              code: "function identity<Type>(arg: Type): Type {\n  return arg;\n}\n\nlet output = identity<string>('Hallo Welt');"
            }
          },
          {
            id: "block-6",
            type: "paragraph",
            data: {
              text: "Es geht hier um vieles um Ideen und um zu lernen und genau das möchte ich damit erreichen! "
            }
          },
        ],
    }
]
