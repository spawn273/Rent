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

FROM alpine
COPY --from=build /app/build-guest /app/guest
COPY --from=build /app/build-emp /app/emp
COPY --from=build /app/build-admin /app/admin
