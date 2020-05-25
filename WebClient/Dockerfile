FROM node:alpine as build
WORKDIR /app
COPY . /app
# install and cache dependencies
RUN yarn install

#build the project for production
RUN yarn build-guest
RUN mv build build-guest

RUN yarn build-emp
RUN mv build build-emp

RUN yarn build-admin
RUN mv build build-admin

FROM nginx:alpine
## Remove default nginx index page
RUN rm -rf /usr/share/nginx/html/*

COPY --from=build /app/build-guest /usr/share/nginx/html/guest
COPY --from=build /app/build-emp /usr/share/nginx/html/emp
COPY --from=build /app/build-admin /usr/share/nginx/html/admin

COPY ./nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80 81 82

ENTRYPOINT ["nginx", "-g", "daemon off;"]
