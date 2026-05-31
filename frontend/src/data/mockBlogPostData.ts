export type HeadingBlock = {
  id: string;
  type: 'heading'; // Der exakte String ist hier der Typ!
  data: {
    level: number;
    text: string;
  };
};

export type TextBlock = {
  id: string;
  type: 'paragraph';
  data: {
    text: string;
  };
};

export type ImageBlock = {
  id: string;
  type: 'image';
  data: {
    src: string;
    alt: string;
    caption?: string;
  };
};

export type CodeBlock = {
  id: string;
  type: 'code';
  data: {
    language: string; // z.B. 'typescript', 'csharp', 'html'
    code: string;
  };
};

export type ContentBlock = HeadingBlock | TextBlock | ImageBlock | CodeBlock;


export type BlogData = {
  id: string,
  contenttype: 'blog' | 'project';
  tags: string[];
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
  CommentsCount?: number; //Anzahl
  comment?: string; // Einzelner Comment im Post selber
}

export const mockBlogData: BlogData[] = [
  {
    id: '762',
    contenttype: 'blog',
    tags: ['typescript', 'generics', 'einfuehrung'],
    slug: "typescript-generics-einfuehrung",
    title: "Einführung in TypeScript Generics",
    author: "Max Mustermann",
    date: new Date("2024-03-15"),
    description: "Eine verständliche Einführung in die Welt der TypeScript Generics mit praktischen Beispielen.",
    imgSrc: "https://media.istockphoto.com/id/816752606/de/foto/tv-testkarte-oder-testmuster-generisch.jpg?s=612x612&w=0&k=20&c=Q4CCpLypL8bfmmlANGkfkpfnYrOSQV6zcLtmIbupVwQ=",
    likes: 42,
    views: 1234,
    CommentsCount: 5,
    comment: "Tolle Einführung! Könntest du auch ein Beispiel mit Constraints zeigen?",

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
        src: "https://media.istockphoto.com/id/816752606/de/foto/tv-testkarte-oder-testmuster-generisch.jpg?s=612x612&w=0&k=20&c=Q4CCpLypL8bfmmlANGkfkpfnYrOSQV6zcLtmIbupVwQ=",
        alt: "Eine Box, die jeden Typ aufnehmen kann",
        caption: "" // Optional, falls du eine Bildunterschrift willst
      }
    },
    {
      id: "block-4",
      type: "heading",
      data: {
        level: 2,
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
    {
      id: "block-7",
      type: "heading",
      data: {
        level: 2,
        text: "Beispiel mit Constraints"
      }
    },
    {
      id: "block-8",
      type: "paragraph",
      data: {
        text: "Generics sind ein mächtiges Werkzeug in TypeScript, um flexible und wiederverwendbare Komponenten zu erstellen. Sie ermöglichen es dir, Funktionen und Klassen zu schreiben, die mit verschiedenen Datentypen arbeiten können, ohne die Typsicherheit zu verlieren."
      }
    },
    {
      id: "block-9",
      type: "heading",
      data: {
        level: 2,
        text: "Constraints in TypeScript Generics"
      }
    },
    {
      id: "block-10",
      type: "paragraph",
      data: {
        text: "Manchmal möchtest du sicherstellen, dass der generische Typ bestimmte Eigenschaften oder Methoden hat. Hier kommen Constraints"
      }
    }
  ],
  }
];
