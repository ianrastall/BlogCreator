using BlogCreator.Core.Interfaces;
using Markdig;

namespace BlogCreator.Infrastructure.Services;

public sealed class MarkdownRenderer : IMarkdownRenderer
{
    private readonly MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    public string RenderDocument(string markdown)
    {
        string body = Markdown.ToHtml(markdown ?? string.Empty, pipeline);

        return $$"""
            <!doctype html>
            <html lang="en">
            <head>
              <meta charset="utf-8">
              <meta name="viewport" content="width=device-width, initial-scale=1">
              <meta http-equiv="Content-Security-Policy" content="default-src 'none'; img-src data: file: https: http:; style-src 'unsafe-inline'; script-src 'none'; object-src 'none'; base-uri 'none'; form-action 'none'; frame-src 'none'">
              <style>
                :root { color-scheme: dark; }
                body {
                  margin: 0;
                  padding: 24px 30px 60px;
                  background: #202020;
                  color: #f2f2f2;
                  font-family: "Segoe UI", sans-serif;
                  font-size: 17px;
                  line-height: 1.65;
                }
                h1, h2, h3, h4 { line-height: 1.2; }
                a { color: #8ab4f8; }
                blockquote {
                  margin-left: 0;
                  padding-left: 16px;
                  border-left: 4px solid #666;
                  color: #d0d0d0;
                }
                code {
                  font-family: "Cascadia Mono", Consolas, monospace;
                  background: #151515;
                  border-radius: 4px;
                  padding: 0.12em 0.3em;
                }
                pre {
                  overflow-x: auto;
                  background: #151515;
                  border: 1px solid #3a3a3a;
                  border-radius: 8px;
                  padding: 16px;
                }
                pre code { padding: 0; }
                img { max-width: 100%; height: auto; }
                table { border-collapse: collapse; }
                th, td { border: 1px solid #555; padding: 8px 12px; }
              </style>
            </head>
            <body>
            {{body}}
            </body>
            </html>
            """;
    }
}
