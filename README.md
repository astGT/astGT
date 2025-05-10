# Azure DevOps MCP Server

A Model Context Protocol (MCP) server that provides tools for interacting with Azure DevOps work items through natural language commands.

## Features

- Query work items using natural language
- Create and update work items
- View work item details and history

## Setup

1. Install dependencies:

```bash
npm install
```

2. Build the project:

```bash
npm run build
```

3. Start the server:

```bash
npm start
```

## Development

For development with watch mode:

```bash
npm run dev
```

## Configuration

The server requires Azure DevOps authentication. You'll need to set up:

- Personal Access Token (PAT)
- Organization URL
- Project name

These can be configured through environment variables or a config file.
