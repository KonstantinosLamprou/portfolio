# build stage
FROM node:lts-alpine as build-stage

WORKDIR /src

COPY package*.json ./

RUN npm install

COPY . .

ARG VITE_API_BASE_URL

ENV VITE_API_BASE_URL=$VITE_API_BASE_URL

RUN npm run build

# production stage
FROM nginx:stable-alpine as production-stage

COPY --from=build-stage /src/dist /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]