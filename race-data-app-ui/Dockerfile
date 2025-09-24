# Stage 1: Build the Angular app
FROM node:20-alpine AS build

WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

COPY . .

RUN npm run build

FROM nginx:alpine

COPY --from=build /app/dist/race-data-app-ui /usr/share/nginx/html
# Expose port 80
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
